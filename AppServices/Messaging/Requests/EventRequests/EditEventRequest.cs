namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class EditEventRequest
{
    public Guid EventId { get; set; }
    public Guid ActorUserId { get; set; } // user performing the edit (must be owner)
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
}
