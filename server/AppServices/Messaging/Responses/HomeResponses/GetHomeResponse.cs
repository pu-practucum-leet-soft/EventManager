using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.Data.Entities;

namespace EventManager.AppServices.Messaging.Responses.HomeResponses
{
    /// <summary>
    /// Отговорът за началната страница, съдържащ обобщена информация за потребителя.
    /// </summary>
    public class GetHomeResponse
    {
        /// <summary>
        /// Списък с предстоящите събития, в които потребителят участва.
        /// </summary>
        public EventViewModel[] UpcomingEvents { get; set; } = new EventViewModel[0];

        /// <summary>
        /// Списък с последно добавените или активни събития.
        /// </summary>
        public EventViewModel[] RecentEvents { get; set; } = new EventViewModel[0];

        /// <summary>
        /// Списък с покани за събития, изпратени към потребителя.
        /// </summary>
        public EventParticipantViewModel[] Invites { get; set; } = new EventParticipantViewModel[0];
    }
}
