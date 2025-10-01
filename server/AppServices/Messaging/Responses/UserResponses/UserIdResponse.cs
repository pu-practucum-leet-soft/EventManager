namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор, който съдържа идентификатора на потребителя.
    /// </summary>
    public class UserIdResponse
    {
        /// <summary>
        /// Уникален идентификатор (GUID) на потребителя.
        /// </summary>
        public Guid Id { get; set; }
    }
}
