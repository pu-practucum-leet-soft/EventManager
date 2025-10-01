namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// Модел, представляващ входни данни за създаване или вписване на потребител.
    /// Служи като DTO за предаване на основна информация за потребителя
    /// между клиентското приложение и сървъра.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Потребителско име на акаунта.
        /// Използва се за идентификация в системата.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Имейл адрес на потребителя.
        /// Трябва да бъде уникален и валиден, използва се за вход и нотификации.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Парола на потребителя.
        /// Подлежи на валидиране и се хешира при съхранение в базата данни.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Външен URL адрес на профилната снимка на потребителя.
        /// </summary>
        public string? ProfileImageUrl { get; set; }
    }
}
