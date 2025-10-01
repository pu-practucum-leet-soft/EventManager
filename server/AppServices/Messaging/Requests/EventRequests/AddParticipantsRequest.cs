namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    /// <summary>
    /// Заявка за добавяне на участници към събитие.
    /// Използва се при изпращане на покани от създателя на събитието.
    /// </summary>
    public class AddParticipantsRequest
    {
        /// <summary>
        /// Уникален идентификатор на събитието, към което се добавят участници.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Идентификатор на потребителя, който извършва действието (трябва да е собственик на събитието).
        /// </summary>
        public Guid ActorUserId { get; set; } // must be owner

        /// <summary>
        /// Списък от идентификатори на потребителите, които ще бъдат поканени като участници.
        /// </summary>
        public IEnumerable<Guid> UserIds { get; set; } = Array.Empty<Guid>();
    }
}
