using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    public class GetInvitesOutcomingResponse : ServiceResponseBase
    {
        public List<EventParticipantViewModel> Invites { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
