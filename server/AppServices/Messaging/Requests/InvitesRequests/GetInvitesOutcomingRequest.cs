using EventManager.AppServices.Messaging.Responses.InvitesResponses;

namespace EventManager.AppServices.Messaging.Requests.InvitesRequests
{
    public class GetInvitesOutcomingRequest : ServiceRequestBase
    {
        public Guid ActingUserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

    }
}
