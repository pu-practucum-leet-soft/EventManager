using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventManager.AppServices.Implementations
{

    /// <summary>
    /// The service works with JWT initializers and credentials.
    /// </summary>
    public class JwtHelper : IJwtHelper
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// JwtHelper service constructor 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        /// <param name="httpContextAccessor"></param>
        public JwtHelper(
        EventManagerDbContext context,
        IConfiguration configuration,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Generates JWT.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<CreateJwtResponse> GenerateJwt(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = ResolveMainRole(roles);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, userRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiryMinutes"]!));

            var jwt = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expiryTime,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new CreateJwtResponse
            {
                Message = "Успешно създаден JWT токън.",
                Token = tokenString,
                TokenExpiryTime = expiryTime,
                StatusCode = BusinessStatusCodeEnum.Success
            };
        }

        /// <summary>
        /// Creates a new Refresh Token.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<RefreshTokenResponse> RenewRefreshToken(RefreshRequest req)
        {
            var response = new RefreshTokenResponse();
            var http = _httpContextAccessor.HttpContext;

            var incoming = http?.Request.Cookies["refresh-token"];
            if (string.IsNullOrEmpty(incoming))
                return new RefreshTokenResponse
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва refresh token."
                };

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == incoming);
            var user = await _userManager.FindByIdAsync(storedToken?.UserId.ToString()!);

            if (storedToken == null || !storedToken.IsActive)
                return new RefreshTokenResponse
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Несъществуваща кредитация."
                };

            var roles = await _userManager.GetRolesAsync(user!);
            var mainRole = ResolveMainRole(roles);

            var jwt = await GenerateJwt(storedToken.User!);

            storedToken.Revoked = DateTime.UtcNow;
            storedToken.RevokedByIp = http?.Connection.RemoteIpAddress?.ToString();

            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_config["RefreshTokenSettings:ExpiryMinutes"]!));
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateSecureRefreshToken(),
                UserId = storedToken.User!.Id,
                Expires = refreshTokenExpiryTime,
                CreatedByIp = http?.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            return new RefreshTokenResponse
            {
                Message = "Токенът беше обновен успешно.",
                StatusCode = BusinessStatusCodeEnum.Success,
                TokenExpiryTime = newRefreshToken.Expires
            };
        }

        /// <summary>
        /// Confirm the Refresh Token is still active.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="revokedByIpString"></param>
        /// <returns></returns>
        public async Task ConfirmRefreshTokenIsAlive(string? refreshToken, string revokedByIpString)
        {
            if (refreshToken != null)
            {
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
                if (storedToken != null)
                {
                    storedToken.Revoked = DateTime.UtcNow;
                    storedToken.RevokedByIp = revokedByIpString;
                    await _context.SaveChangesAsync();
                }
            }
        }

        private static string GenerateSecureRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ResolveMainRole(IList<string> roles)
            => roles.Contains("Admin") ? "Admin" : "User";
    }
}
