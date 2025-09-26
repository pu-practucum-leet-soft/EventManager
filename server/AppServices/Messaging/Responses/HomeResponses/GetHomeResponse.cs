

using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.Data.Entities;

namespace EventManager.AppServices.Messaging.Responses.HomeResponses;

public class GetHomeResponse
{
    public EventViewModel[] UpcomingEvents { get; set; } = new EventViewModel[0];
    public EventViewModel[] RecentEvents { get; set; } = new EventViewModel[0];
    public EventParticipantViewModel[] Invites { get; set; } = new EventParticipantViewModel[0];
}