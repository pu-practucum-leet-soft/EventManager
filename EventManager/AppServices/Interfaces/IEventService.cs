namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

public interface IEventService
{
    //��������� �� �����
    Task<CreateEventResponse> SaveEventAsync(CreateEventRequest req, Guid ownerId);

    //����������� �� �����
    Task<EditEventResponse> UpdateEventAsync(EditEventRequest req, Guid actingUserId, Guid IdEvent);


    Task<EditEventResponse> StatusChangeEventAsync(EditEventRequest req, Guid actingUserId, Guid IdEvent);

    //������ ������
    Task<GetEventsResponse> LoadEventsAsync(GetEventsRequest req);


    //������ ���� ��� ������
    Task<AddParticipantsResponse> SaveParticipantsAsync(AddParticipantsRequest req);




    // ������ �� �������
    Task<GetEventsByTitleResponse> LoadEventsByTitleAsync(GetEventsByTitleRequests req);

    Task<GetEventsByLocationResponse> LoadEventsByLocationAsync(GetEventsByLocationRequests req);

    Task<GetEventsByDateResponse> LoadEventsByDateAsync(GetEventsByDateRequests req);

    Task<GetEventByIdResponse> LoadEventByIdAsync(GetEventByIdRequest req, Guid actingUserId);

    
    Task<GetEventsResponse> LoadMyOwnedEventsAsync(GetEventsRequest req);

    Task<GetEventsResponse> LoadMyEventsPraticipateAsync(GetEventsRequest req);

   

    //EventViewModel GetEvent(GetEventRequest req);
    //EditEventResponse EditEvent(EditEventRequest req);
    //AddParticipantsResponse AddParticipants(AddParticipantsRequest req);
}
