namespace EventManager.AppServices.Messaging.Requests.EventRequests;

using EventManager.Data.Enums;

public class EditEventRequest
{
    public Guid EventId { get; set; }
    public Guid ActorUserId { get; set; } // user performing the edit (must be owner)
    public string? Title { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public EventStatus Status{ get; set; }
}
