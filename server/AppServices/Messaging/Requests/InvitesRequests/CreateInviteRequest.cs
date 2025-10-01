namespace EventManager.AppServices.Messaging.Requests.InvitesRequests
{
    /// <summary>
    /// Заявка за създаване на нова покана към събитие.
    /// </summary>
    public class CreateInviteRequest : ServiceRequestBase
    {
        /// <summary>
        /// Имейл адрес на потребителя, който трябва да получи поканата.
        /// </summary>
        public string? InviteeEmail { get; set; }
    }
}
