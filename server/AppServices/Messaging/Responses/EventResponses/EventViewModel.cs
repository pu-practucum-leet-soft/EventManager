using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Enums;

namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;


    public string? Description { get; set; }


    public DateTime? StartDate { get; set; }

    public string? Location { get; set; }

    public bool Public { get; set; }
    public string? Email { get; set; }

    public bool Joined  { get; set; }
    public UserViewModel Owner { get; set; }

    public EventStatus Status { get; set; }

    public int? ParticipantCount {  get; set; }

    public ICollection<UserViewModel> Participants { get; set; } = new List<UserViewModel>();



}
