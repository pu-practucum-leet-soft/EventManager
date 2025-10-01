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
    /// Сервизен клас, който работи с JWT и Refresh токени.
    /// Отговаря за генериране, подновяване и валидиране на токени за удостоверяване.
    /// </summary>
    public class JwtHelper : IJwtHelper
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Конструктор, който инициализира <see cref="JwtHelper"/> 
        /// и инжектира необходимите зависимости.
        /// </summary>
        /// <param name="context">Базата данни за достъп до Refresh токени.</param>
        /// <param name="configuration">Конфигурация с JwtSettings.</param>
        /// <param name="userManager">Управление на потребителите в системата.</param>
        /// <param name="httpContextAccessor">Достъп до текущия HTTP контекст.</param>
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
        /// Генерира нов JWT за даден потребител.
        /// </summary>
        /// <param name="user">Потребителят, за когото се генерира токена.</param>
        /// <returns>Отговор с токена и информация за неговата валидност.</returns>
        public async Task<CreateJwtResponse> GenerateJwt(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = ResolveMainRole(roles);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("Role", userRole),
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
        /// Подновява Refresh токен и издава нов JWT.
        /// </summary>
        /// <param name="req">Заявка, съдържаща текущия Refresh токен.</param>
        /// <returns>Отговор със статус и нов Refresh токен.</returns>
        public async Task<RefreshTokenResponse> RenewRefreshToken()
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
            storedToken.RevokedByIp = http!.Connection.RemoteIpAddress?.ToString();

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


            http!.Response.Cookies.Append("refresh-token", newRefreshToken.Token, new CookieOptions
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

        /// <summary>
        /// Маркира Refresh токен като невалиден (revoked).
        /// </summary>
        /// <param name="refreshToken">Стойността на токена, който трябва да бъде проверен/анулиран.</param>
        /// <param name="revokedByIpString">IP адресът, от който е извършено анулирането.</param>
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

        /// <summary>
        /// Генерира криптографски сигурен низ за Refresh токен.
        /// </summary>
        private static string GenerateSecureRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Определя основната роля на потребителя (Admin или User).
        /// </summary>
        private static string ResolveMainRole(IList<string> roles)
            => roles.Contains("Admin") ? "Admin" : "User";
    }
}
