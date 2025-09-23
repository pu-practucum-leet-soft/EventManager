using EventManager.AppServices.Messaging.Responses.UserResponses;

namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Location { get; set; }
    public string? Notes { get; set; }

    public DateTime? StartDate { get; set; }
    public UserViewModel Owner { get; set; }
    public List<Guid> ParticipantUserIds { get; set; } = new();
}
