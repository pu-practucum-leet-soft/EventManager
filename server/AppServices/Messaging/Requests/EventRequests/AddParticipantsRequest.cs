namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class AddParticipantsRequest
{
    public Guid EventId { get; set; }
    public Guid ActorUserId { get; set; } // must be owner
    public IEnumerable<Guid> UserIds { get; set; } = Array.Empty<Guid>();
}
