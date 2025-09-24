namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.Controllers;

public interface IEventService
{
    Task<CreateEventResponse> CreateEvent(CreateEventRequest req, string userId);
    Task<EventViewModel> GetEvent(GetEventRequest req);
    Task<EditEventResponse> EditEvent(EditEventRequest req);
    Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req, string inviterId);
    Task<GetAllEventsResponse> GetAllEvents();
    Task<StatisticViewModel> GetEventStatistic(Guid ownerId);
}
