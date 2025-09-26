namespace EventManager.AppServices.Interfaces
{
    using EventManager.AppServices.Messaging.Responses.UserResponses;
    using EventManager.AppServices.Messaging.Requests.UserRequests;
    using EventManager.Data.Entities;

    public interface IJwtHelper
    {
        Task<CreateJwtResponse> GenerateJwt(User user);
        Task<RefreshTokenResponse> RenewRefreshToken(RefreshRequest req);
        Task ConfirmRefreshTokenIsAlive(string? refreshToken, string revokedByIpString);
    }
}
