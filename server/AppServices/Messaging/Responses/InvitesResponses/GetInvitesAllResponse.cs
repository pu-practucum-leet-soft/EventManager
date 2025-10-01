using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses;

public class GetInvitesAllResponse : ServiceResponseBase
{
    public List<EventParticipantViewModel> IncomingInvites { get; set; } = new List<EventParticipantViewModel>();
    public List<EventParticipantViewModel> OutcomingInvites { get; set; } = new List<EventParticipantViewModel>();
}