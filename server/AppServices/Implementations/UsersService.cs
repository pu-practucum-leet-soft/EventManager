using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace EventManager.AppServices.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="context">Movie database context.</param>

        public UsersService(
            EventManagerDbContext context,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateUserResponse> SaveAsync(CreateUserRequest request)
        {
            CreateUserResponse response = new();

            try
            {
                var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.User.Email);

                if (existingUser != null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.Conflict;
                    return response;
                }

                var user = new User
                {
                    UserName = request.User.UserName,
                    Email = request.User.Email,
                    PasswordHash = request.User.Password,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                response.StatusCode = BusinessStatusCodeEnum.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                return response;
            }
            return response;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            LoginResponse response = new();

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                response.StatusCode = BusinessStatusCodeEnum.BadRequest;
                response.Message = "Невалидна заявка.";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Message = "Invalid login attempt.";
                return response;
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                response.Message = "Invalid login attempt.";
                return response;
            }

            try
            {
                //var user = await _context.Users
                //    .FirstOrDefaultAsync(u =>
                //        u.Email == request.Email &&
                //        u.PasswordHash == request.Password);

                //if (user == null)
                //{
                //    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                //    response.Message = "Невалиден имейл или парола.";
                //    return response;
                //}

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.First();
                // 👉 Тук вече имаш пълен списък с роли
                // Можеш да ги вкараш като Claims в cookie/JWT
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!)
                };

                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var claimsIdentity = new ClaimsIdentity(claims, "Identity.Application");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Създаване на cookie (ако ползваш cookie auth)
                await _signInManager.SignInAsync(user, isPersistent: false);

                response.StatusCode = BusinessStatusCodeEnum.Success;
                response.Token = JwtHelper.GenerateJwtToken(user.Id.ToString(), role, _configuration);

                return new LoginResponse
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = role,
                    Id = Guid.Parse(user.Id),
                    Token = response.Token,

                };

            }
            catch (Exception ex)
            {
                Console.WriteLine("LOGIN ERROR: " + ex.Message);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
            }

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
    }
}
