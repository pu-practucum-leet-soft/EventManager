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


    public class JwtHelper : IJwtHelper
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="context">Movie database context.</param>

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

        public async Task<CreateJwtResponse> GenerateJwt(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = ResolveMainRole(roles);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("Role", userRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiryMinutes"]));

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

        public async Task<RefreshTokenResponse> RenewRefreshToken(RefreshRequest req)
        {
            var response = new RefreshTokenResponse();
            var http = _httpContextAccessor.HttpContext;

            var incoming = http.Request.Cookies["refresh-token"];
            if (string.IsNullOrEmpty(incoming))
                return new RefreshTokenResponse
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва refresh token."
                };

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == incoming);
            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

            if (storedToken == null || !storedToken.IsActive)
                return new RefreshTokenResponse
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Несъществуваща кредитация."
                };

            var roles = await _userManager.GetRolesAsync(user);
            var mainRole = ResolveMainRole(roles);

            var jwt = await GenerateJwt(storedToken.User);

            storedToken.Revoked = DateTime.UtcNow;
            storedToken.RevokedByIp = http.Connection.RemoteIpAddress?.ToString();

            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_config["RefreshTokenSettings:ExpiryMinutes"]));
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateSecureRefreshToken(),
                UserId = storedToken.User.Id,
                Expires = refreshTokenExpiryTime,
                CreatedByIp = http.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            http.Response.Cookies.Append("jwt-token", jwt.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = jwt.TokenExpiryTime
            });
            http.Response.Cookies.Append("refresh-token", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = newRefreshToken.Expires
            });

            return new RefreshTokenResponse
            {
                Message = "Токенът беше обновен успешно.",
                StatusCode = BusinessStatusCodeEnum.Success,
                Token = jwt.Token,
                TokenExpiryTime = newRefreshToken.Expires
            };
        }

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
