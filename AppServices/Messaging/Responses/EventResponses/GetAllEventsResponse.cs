namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventSummary
{
    public string Name { get; set; } = default!;
    public string Location { get; set; }
    public string DateTime { get; set; }
}

public class GetAllEventsResponse
{
    public List<EventSummary> Events { get; set; } = new();
}
