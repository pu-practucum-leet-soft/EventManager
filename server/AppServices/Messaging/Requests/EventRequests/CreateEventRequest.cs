namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    /// <summary>
    /// Заявка за създаване на ново събитие.
    /// Използва се при подаване на данни от клиента към API-то за регистриране на ново събитие.
    /// </summary>
    public class CreateEventRequest
    {
        /// <summary>
        /// Заглавие/име на събитието (задължително поле).
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Местоположение, на което ще се проведе събитието (по желание).
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Описание или бележки за събитието (по желание).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Дата и час на стартиране на събитието (по желание).
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Уникален идентификатор на потребителя, който създава събитието (собственик).
        /// </summary>
        public Guid OwnerUserId { get; set; }
    }
}
