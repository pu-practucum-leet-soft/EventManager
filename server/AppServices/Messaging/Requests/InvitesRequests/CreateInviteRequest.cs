namespace EventManager.AppServices.Messaging.Requests.InvitesRequests;

public class CreateInviteRequest : ServiceRequestBase
{
    public Guid InviteeId { get; set; }
    public Guid EventId { get; set; }
}
