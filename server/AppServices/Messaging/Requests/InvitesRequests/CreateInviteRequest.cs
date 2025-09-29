namespace EventManager.AppServices.Messaging.Requests.InvitesRequests;

public class CreateInviteRequest : ServiceRequestBase
{
    public string InviteeEmail { get; set; }
}
