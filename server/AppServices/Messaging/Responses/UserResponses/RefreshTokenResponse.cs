namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор при обновяване на токен за достъп (refresh token).
    /// </summary>
    public class RefreshTokenResponse : ServiceResponseBase
    {
        /// <summary>
        /// Уникален идентификатор на refresh заявката или записа.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Допълнително съобщение, описващо резултата от операцията.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Новоиздаденият JWT токен.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Дата и час, до които новият токен е валиден.
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
