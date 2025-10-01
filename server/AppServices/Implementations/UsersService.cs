using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace EventManager.AppServices.Implementations
{
    /// <summary>
    /// Инициализира инстанция на <see cref="UsersService"/>, която управлява
    /// потребителските действия – регистрация, вход, изход и роли.
    /// </summary>
    /// <param name="context">
    /// Контекстът на базата данни (<see cref="EventManagerDbContext"/>), използван за достъп и
    /// промяна на потребители, роли и токени.
    /// </param>
    /// <param name="configuration">
    /// Конфигурационен обект (<see cref="IConfiguration"/>), съдържащ настройки за JWT и други параметри.
    /// </param>
    /// <param name="userManager">
    /// Сервизът (<see cref="UserManager{TUser}"/>) за управление на потребителите,
    /// който предоставя операции като създаване, търсене и проверка на пароли.
    /// </param>
    /// <param name="httpContextAccessor">
    /// Достъп до текущия HTTP контекст (<see cref="IHttpContextAccessor"/>),
    /// използван за работа с заявки и отговори, включително бисквитки.
    /// </param>
    /// <param name="jwtHelper">
    /// Помощен сервиз (<see cref="IJwtHelper"/>), отговорен за генерирането и подновяването на JWT и Refresh токени.
    /// </param>
    public class UsersService(
        EventManagerDbContext context,
        IConfiguration configuration,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,
        IJwtHelper jwtHelper) : IUsersService
    {
        private readonly EventManagerDbContext _context = context;
        private readonly IConfiguration _config = configuration;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtHelper _jwtHelper = jwtHelper;

        /// <summary>
        /// Регистрира нов потребител в системата.
        /// </summary>
        /// <param name="request">Заявка за създаване на потребител.</param>
        /// <returns>Резултат от регистрацията.</returns>
        public async Task<CreateUserResponse> SaveAsync(CreateUserRequest request)
        {
            CreateUserResponse response = new();

            var existingUser = await _userManager.FindByEmailAsync(request.User.Email!);

            if (existingUser is not null)
            {
                response.StatusCode = BusinessStatusCodeEnum.Conflict;
                return response;
            }

            var user = new User
            {
                UserName = request.User.UserName,
                Email = request.User.Email
            };

            var create = await _userManager.CreateAsync(user, request.User.Password!);
            if (create.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                response.StatusCode = BusinessStatusCodeEnum.Success;
                return response;
            }

            response.StatusCode = BusinessStatusCodeEnum.BadRequest;
            response.Message = string.Join("; ", create.Errors.Select(e => e.Description));
            return response;

        }

        /// <summary>
        /// Връща информация за потребител по неговото Id.
        /// </summary>
        /// <param name="id">Уникален идентификатор на потребителя.</param>
        /// <returns>Модел с името и имейла на потребителя.</returns>
        public async Task<UserViewModel?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
            if (user == null) return null;

            return new UserViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!
            };
        }

        /// <summary>
        /// Намира потребител по имейл и връща неговия идентификатор.
        /// </summary>
        /// <param name="email">Имейл адрес на потребителя.</param>
        /// <returns>Обект със стойност Id.</returns>
        /// <exception cref="Exception">Хвърля се, ако потребителят не е намерен.</exception>
        public async Task<UserIdResponse> GetUserIdByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("User not found");

            return new UserIdResponse { Id = user.Id };
        }

        /// <summary>
        /// Вписва потребител чрез имейл и парола.
        /// При успех генерира JWT и Refresh токен.
        /// </summary>
        /// <param name="request">Заявка с имейл и парола.</param>
        /// <returns>Резултат от вписването с токени и данни за потребителя.</returns>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = new LoginResponse();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new LoginResponse
                {
                    Email = request.Email,
                    Message = "Въведените имейл и парола не съвпадат със съшествуващ потребител.",
                    StatusCode = BusinessStatusCodeEnum.BadRequest,
                    UserName = "Несъществуващ потребител"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            // В случай, че на потребителя са назначени 2 роли (Admin или User) в JWT ще се генерира основната
            var mainRole = ResolveMainRole(roles);

            var jwt = await _jwtHelper.GenerateJwt(user);

            var refreshExpiryMins = int.Parse(_config["RefreshTokenSettings:ExpiryMinutes"]!);
            var refresh = new RefreshToken
            {
                Token = GenerateSecureRefreshToken(),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddMinutes(refreshExpiryMins),
                CreatedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };

            await _context.RefreshTokens.AddAsync(refresh);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = mainRole,
                Token = jwt.Token!,
                TokenExpiryTime = jwt.TokenExpiryTime,
                Message = $"Вписването във вашият профил премина успешно. Добре дошли, {user.UserName}",
                StatusCode = BusinessStatusCodeEnum.Success
            };
        }

        /// <summary>
        /// Изписва потребител, като анулира неговия Refresh токен.
        /// </summary> 
        public async Task LogoutAsync()
        {
            var http = _httpContextAccessor.HttpContext!;
            var rt = http.Request.Cookies["refresh-token"];

            if (!string.IsNullOrEmpty(rt))
            {
                var stored = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == rt);
                if (stored is not null)
                {
                    stored.Revoked = DateTime.UtcNow;
                    stored.RevokedByIp = http.Connection.RemoteIpAddress?.ToString();
                    await _context.SaveChangesAsync();
                }
            }
            http.Response.Cookies.Delete("jwt-token");
            http.Response.Cookies.Delete("refresh-token");
        }

        /// <summary>
        /// Назначава роля на съществуващ потребител.
        /// </summary>
        /// <param name="userId">Идентификатор на потребителя.</param>
        /// <param name="roleName">Име на ролята.</param>
        /// <returns>Съобщение за резултата.</returns>
        public async Task<string> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
                return "Role assigned";
            else
                return new StringBuilder().Append(result.Errors).ToString();
        }

        /// <summary>
        /// Генерира криптографски сигурен Refresh токен.
        /// </summary>
        private static string GenerateSecureRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Определя основната роля на потребителя – Admin или User.
        /// </summary>
        private static string ResolveMainRole(IList<string> roles)
            => roles.Contains("Admin") ? "Admin" : "User";
    }
}
