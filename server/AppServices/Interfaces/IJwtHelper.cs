namespace EventManager.AppServices.Interfaces
{
    using EventManager.AppServices.Messaging.Responses.UserResponses;
    using EventManager.Data.Entities;

    public interface IJwtHelper
    {
        Task<CreateJwtResponse> GenerateJwt(User user);
        Task<RefreshTokenResponse> RenewRefreshToken();
        Task ConfirmRefreshTokenIsAlive(string? refreshToken, string revokedByIpString);
    }
}
