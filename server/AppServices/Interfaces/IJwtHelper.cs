namespace EventManager.AppServices.Interfaces
{
    using EventManager.AppServices.Messaging.Responses.UserResponses;
    using EventManager.AppServices.Messaging.Requests.UserRequests;
    using EventManager.Data.Entities;

    /// <summary>
    /// Интерфейс за помощни методи, свързани с управлението на JWT и Refresh токени.
    /// Отговаря за създаване на JWT, подновяване на Refresh токени и валидиране на съществуващи токени.
    /// </summary>
    public interface IJwtHelper
    {
        /// <summary>
        /// Генерира нов JWT токен за конкретен потребител.
        /// </summary>
        /// <param name="user">Потребителят, за когото се създава токенът.</param>
        /// <returns>Отговор със създадения JWT токен и времето на изтичане.</returns>
        Task<CreateJwtResponse> GenerateJwt(User user);

        /// <summary>
        /// Подновява Refresh токен, ако е валиден, и връща нов JWT и Refresh токен.
        /// </summary>
        /// <param name="req">Заявка, съдържаща стария Refresh токен.</param>
        /// <returns>Отговор със статус и информация за новите токени.</returns>
        Task<RefreshTokenResponse> RenewRefreshToken(/*RefreshRequest req*/);

        /// <summary>
        /// Потвърждава, че даден Refresh токен е бил отнет (revoked),
        /// като записва кога и от кой IP адрес е отнет.
        /// </summary>
        /// <param name="refreshToken">Стойността на Refresh токена, който трябва да се анулира.</param>
        /// <param name="revokedByIpString">IP адресът, от който е анулиран токенът.</param>
        /// <returns>Задача без резултат, извършваща актуализацията в базата.</returns>
        Task ConfirmRefreshTokenIsAlive(string? refreshToken, string revokedByIpString);
    }
}
