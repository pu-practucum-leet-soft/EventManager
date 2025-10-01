using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    /// <summary>
    /// Отговор за зареждане на входящи покани (получени от текущия потребител).
    /// Съдържа списък с поканите и информация за страниране.
    /// </summary>
    public class GetInvitesIncomingResponse : ServiceResponseBase
    {
        /// <summary>
        /// Колекция от входящи покани за събития.
        /// </summary>
        public List<EventParticipantViewModel> Invites { get; set; } = new();

        /// <summary>
        /// Общият брой входящи покани в базата за дадения потребител.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Номер на текущата страница при страниране.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Размер на страницата (брой елементи на страница).
        /// </summary>
        public int PageSize { get; set; }
    }
}
