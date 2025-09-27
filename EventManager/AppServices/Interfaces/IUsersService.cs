using System.Security.Claims;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;

namespace EventManager.AppServices.Interfaces
{
    public interface IUsersService
    {
        /// <summary>
        /// Get list with movies.
        /// </summary>
        /// <param name="request">Get movie request object.</param>
        /// <returns>Return filter list with movies.</returns>
       // Task<GetMovieResponse> GetMovieAsync(GetMovieRequest request);


        /// <summary>
        /// Create User.
        /// </summary>
        /// <param name="request">Create user request object.</param>
        /// <returns>Return 200 ok.</returns>
        Task<CreateUserResponse> SaveAsync(CreateUserRequest request);


        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<UserViewModel?> GetUserByIdAsync(string id);

        Task<UserIdResponse> GetUserIdByEmailAsync(string email);

    }
}
