namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

public interface IEventService
{
    CreateEventResponse CreateEvent(CreateEventRequest req);
    EventViewModel GetEvent(GetEventRequest req);
    EditEventResponse EditEvent(EditEventRequest req);
    AddParticipantsResponse AddParticipants(AddParticipantsRequest req);
    GetAllEventsResponse GetAllEvents(GetAllEventsRequest req);
}
