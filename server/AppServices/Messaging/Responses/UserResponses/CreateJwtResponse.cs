namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор при създаване на нов JWT токен.
    /// </summary>
    public class CreateJwtResponse : ServiceResponseBase
    {
        /// <summary>
        /// Уникален идентификатор, свързан с потребителя или операцията.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Информационно съобщение за резултата от операцията.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Генерираният JWT токен като низ.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Дата и час на изтичане на валидността на токена.
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
