namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    /// <summary>
    /// Заявка за извличане на конкретно събитие по неговия идентификатор.
    /// </summary>
    public class GetEventRequest
    {
        /// <summary>
        /// Уникален идентификатор на събитието, което трябва да бъде намерено.
        /// </summary>
        public Guid EventId { get; set; }
    }
}
