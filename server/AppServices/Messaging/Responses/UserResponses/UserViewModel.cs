namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Представяне на потребител с основна информация,
    /// която може да се използва във визуализации и отговори на заявки.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Потребителско име на потребителя.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Имейл адрес на потребителя.
        /// </summary>
        public string? Email { get; set; }
    }
}
