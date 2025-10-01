namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор от сървиса при опит за вход (login) на потребител.
    /// </summary>
    public class LoginResponse : ServiceResponseBase
    {
        /// <summary>
        /// Уникален идентификатор на потребителя.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Съобщение, описващо резултата от опита за вход.
        /// </summary>
        public new string? Message { get; set; }

        /// <summary>
        /// Потребителското име на автентикирания потребител.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Имейл адресът на потребителя.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Издаденият JWT токен за автентикация.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Основната роля на потребителя (например "User" или "Admin").
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Дата и час, до които токенът е валиден.
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
