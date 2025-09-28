namespace EventManager.AppServices.Interfaces
{
    using EventManager.AppServices.Messaging.Responses.UserResponses;
    using EventManager.AppServices.Messaging.Requests.UserRequests;
    using EventManager.Data.Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface IJwtHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<CreateJwtResponse> GenerateJwt(User user);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<RefreshTokenResponse> RenewRefreshToken(RefreshRequest req);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="revokedByIpString"></param>
        /// <returns></returns>
        Task ConfirmRefreshTokenIsAlive(string? refreshToken, string revokedByIpString);
    }
}
