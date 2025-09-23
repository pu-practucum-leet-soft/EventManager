namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class CreateEventRequest
{
    public string Name { get; set; } = default!;
    public string? Location { get; set; }
    public string? Notes { get; set; }

    public DateTime? StartDate { get; set; }
    public Guid OwnerUserId { get; set; }
}
