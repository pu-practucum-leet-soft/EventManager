using System.Security.Claims;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;

namespace EventManager.AppServices.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Create User.
        /// </summary>
        /// <param name="request">Create user request object.</param>
        /// <returns>Return 200 ok.</returns>
        Task<CreateUserResponse> SaveAsync(CreateUserRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel?> GetUserByIdAsync(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserIdResponse> GetUserIdByEmailAsync(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<string> AssignRole(string userId, string roleName);
    }
}
