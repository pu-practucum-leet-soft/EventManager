namespace EventManager.AppServices.Messaging.Responses.EventResponses;

using EventManager.Data.Entities;
using EventManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EventSummary
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Location { get; set; }
    public Guid OwnerUserId { get; set; }
    public EventStatus Status { get; set; }
    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}

public class GetAllEventsResponse
{
    public List<EventSummary> Events { get; set; } = new();
}
