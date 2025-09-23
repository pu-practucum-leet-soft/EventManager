namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

public interface IEventService
{
    Task<CreateEventResponse> CreateEvent(CreateEventRequest req);
    Task<EventViewModel> GetEvent(GetEventRequest req);
    Task<EditEventResponse> EditEvent(EditEventRequest req);
    Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req);
    Task<GetAllEventsResponse> GetAllEvents();
}
