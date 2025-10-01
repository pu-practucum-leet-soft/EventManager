namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор при създаване на нов потребител.
    /// </summary>
    public class CreateUserResponse : ServiceResponseBase
    {
        /// <summary>
        /// Информационно съобщение за резултата от операцията.
        /// </summary>
        public string? Message { get; set; }
    }
}
