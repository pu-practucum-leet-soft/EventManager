namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Location { get; set; }
}

public class GetAllEventsResponse
{
    public List<EventSummary> Events { get; set; } = new();
}
