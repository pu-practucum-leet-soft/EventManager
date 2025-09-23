namespace EventManager.AppServices.Messaging.Responses.EventResponses;

using EventManager.Data.Entities;

public class EventSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Location { get; set; }
    public string Notes { get; set; }

    // Currently counts the number of participants. Later we could redo it to work with the full Participant models.
    public int ParticipantsCount { get; set; }
}

public class GetAllEventsResponse
{
    public List<EventSummary> Events { get; set; } = new();
}
