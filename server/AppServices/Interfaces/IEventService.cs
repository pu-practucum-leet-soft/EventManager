namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

public interface IEventService
{
    //Създаване на евент
    Task<CreateEventResponse> SaveEventAsync(CreateEventRequest req, Guid ownerId);

    //Редактиране на евент
    Task<EditEventResponse> UpdateEventAsync(EditEventRequest req, Guid actingUserId, Guid IdEvent);

    //Всички евенти
    Task<GetEventsResponse> LoadEventsAsync(GetEventsRequest req);


    //Добави хора към евента
    Task<AddParticipantsResponse> SaveParticipantsAsync(AddParticipantsRequest req);




    // Филтри за търсене
    Task<GetEventsByTitleResponse> LoadEventsByTitleAsync(GetEventsByTitleRequests req);

    Task<GetEventByIdResponse> LoadEventByIdAsync(GetEventByIdRequest req, Guid actingUserId);

    
    Task<GetEventsResponse> LoadMyOwnedEventsAsync(GetEventsRequest req);

    Task<GetEventsResponse> LoadMyEventsPraticipateAsync(GetEventsRequest req);

   

    //EventViewModel GetEvent(GetEventRequest req);
    //EditEventResponse EditEvent(EditEventRequest req);
    //AddParticipantsResponse AddParticipants(AddParticipantsRequest req);
}
