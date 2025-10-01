using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Enums;

namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventParticipantViewModel
{
    public Guid Id { get; set; }
    public EventViewModel Event { get; set; }
    public InviteStatus Status { get; set; }

    public UserViewModel? Invitee { get; set; }
    public UserViewModel? Inviter { get; set; }
}


