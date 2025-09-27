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
    public class UsersService : IUsersService
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtHelper _jwtHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="context">Movie database context.</param>

        public UsersService(
            EventManagerDbContext context,
            IConfiguration configuration,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IJwtHelper jwtHelper)
        {
            _context = context;
            _config = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
        }

        public async Task<CreateUserResponse> SaveAsync(CreateUserRequest request)
        {
            CreateUserResponse response = new();

            var existingUser = await _userManager.FindByEmailAsync(request.User.Email);

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

            var create = await _userManager.CreateAsync(user, request.User.Password);
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

        public async Task<UserViewModel?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
            if (user == null) return null;

            return new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<UserIdResponse> GetUserIdByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("User not found");

            return new UserIdResponse { Id = user.Id };
        }

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
                    UserName = user != null ? user?.UserName : "Несъществуващ потребител"
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

            var http = _httpContextAccessor.HttpContext!;
            http.Response.Cookies.Append("jwt-token", jwt.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = jwt.TokenExpiryTime
            });

            http.Response.Cookies.Append("refresh-token", refresh.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = refresh.Expires
            });

            return new LoginResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = mainRole,
                Token = jwt.Token,
                TokenExpiryTime = jwt.TokenExpiryTime,
                Message = $"Вписването във вашият профил премина успешно. Добре дошли, {user.UserName}",
                StatusCode = BusinessStatusCodeEnum.Success
            };
        }

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
