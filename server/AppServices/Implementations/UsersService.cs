using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;

namespace EventManager.AppServices.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly EventManagerDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="context">Movie database context.</param>


        public UsersService(EventManagerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Email = request.User.Email,
                    PasswordHash = request.User.Password,
                    Role = "user"
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

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Email == request.Email &&
                        u.PasswordHash == request.Password);

                if (user == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "Невалиден имейл или парола.";
                    return response;
                }



                response.StatusCode = BusinessStatusCodeEnum.Success;
                response.Token = JwtHelper.GenerateJwtToken(user.Id.ToString(), user.Role, _configuration);

                return new LoginResponse
                {
                    FristName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Id = user.Id,
                    Token = response.Token,
                    StatusCode = response.StatusCode
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
                FirstName = user.FirstName,
                LastName = user.LastName,
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

    }
}
