using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    /// <summary>
    /// Отговорът съдържа всички покани, свързани с даден потребител –
    /// входящи (получени) и изходящи (изпратени).
    /// </summary>
    public class GetInvitesAllResponse : ServiceResponseBase
    {
        /// <summary>
        /// Списък с входящите покани за събития, получени от потребителя.
        /// </summary>
        public List<EventParticipantViewModel> IncomingInvites { get; set; } = new List<EventParticipantViewModel>();

        /// <summary>
        /// Списък с изходящите покани за събития, изпратени от потребителя.
        /// </summary>
        public List<EventParticipantViewModel> OutcomingInvites { get; set; } = new List<EventParticipantViewModel>();
    }
}
