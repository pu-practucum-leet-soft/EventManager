namespace EventManager.AppServices.Messaging.Requests.InvitesRequests
{
    /// <summary>
    /// Заявка за извличане на изходящи покани (изпратени от текущия потребител).
    /// </summary>
    public class GetInvitesOutcomingRequest : ServiceRequestBase
    {
        /// <summary>
        /// Идентификатор на потребителя, който изпраща поканите.
        /// </summary>
        public Guid ActingUserId { get; set; }

        /// <summary>
        /// Номер на страницата за страниране. По подразбиране е 1.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Размер на страницата (брой записи за зареждане). По подразбиране е 20.
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
