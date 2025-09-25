namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

public interface IEventService
{
    //��������� �� �����
    Task<CreateEventResponse> SaveEventAsync(CreateEventRequest req, Guid ownerId);

    //����������� �� �����
    Task<EditEventResponse> UpdateEventAsync(EditEventRequest req, Guid actingUserId);

    //������ ������
    Task<GetEventsResponse> LoadEventsAsync(GetEventsRequest req);


    //������ ���� ��� ������
    Task<AddParticipantsResponse> SaveParticipantsAsync(AddParticipantsRequest req, Guid actingUserId);




    // ������ �� �������
    Task<GetEventByIdResponse> LoadEventByIdAsync(GetEventByIdRequest req, Guid actingUserId, bool isAdmin);
    Task<GetEventsResponse> LoadMyOwnedEventsAsync(GetEventsRequest req);

    Task<GetEventsResponse> LoadMyEventsPraticipateAsync(GetEventsRequest req);

    //EventViewModel GetEvent(GetEventRequest req);
    //EditEventResponse EditEvent(EditEventRequest req);
    //AddParticipantsResponse AddParticipants(AddParticipantsRequest req);
}
