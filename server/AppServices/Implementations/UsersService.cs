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
    /// Initializes the user service which manages the user actions.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="userManager"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="jwtHelper"></param>
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
        /// Registers a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        /// Gets a user from the database and returns the view model
        /// </summary>
        /// <param name="id">User data</param>
        /// <returns>UserViewModel</returns>
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
        /// Gets a user from the database and returns the view model
        /// </summary>
        /// <param name="email">User information - email</param>
        /// <returns></returns>
        /// <exception cref="Exception">If the user is not found throws an Exception.</exception>
        public async Task<UserIdResponse> GetUserIdByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("User not found");

            return new UserIdResponse { Id = user.Id };
        }

        /// <summary>
        /// Signs in the user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>LoginResponse</returns>
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


            var http = _httpContextAccessor.HttpContext!;
            http.Response.Cookies.Append("refresh-token", refresh.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = refresh.Expires
            });


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
        /// Signs the user out.
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
        /// Adds a role for the user
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <param name="roleName">The selected role</param>
        /// <returns></returns>
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
