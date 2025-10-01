namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    /// <summary>
    /// Заявка за извличане на всички събития.
    /// Може да върне всички събития в системата или да се филтрира по конкретен собственик.
    /// </summary>
    public class GetAllEventsRequest
    {
        /// <summary>
        /// Уникален идентификатор на потребител (собственик на събития).
        /// Ако е зададен, ще се върнат само събитията създадени от този потребител.
        /// Ако е null, ще се върнат всички налични събития.
        /// </summary>
        public Guid? OwnerUserId { get; set; }
    }
}
