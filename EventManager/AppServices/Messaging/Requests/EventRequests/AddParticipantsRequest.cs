namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class AddParticipantsRequest : ServiceRequestBase
{
    public Guid EventId { get; set; }
    public Guid ActorUserId { get; set; } // must be owner
    public List<Guid> UserIds { get; set; } = new();
}
