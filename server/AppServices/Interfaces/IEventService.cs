namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

/// <summary>
/// The interface for the event service. Serves to contain the business logic for handling the events.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<CreateEventResponse> CreateEvent(CreateEventRequest req, string userId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<EventViewModel> GetEvent(GetEventRequest req);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<EditEventResponse> EditEvent(Guid eventId, Guid userId, EditEventRequest req);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="inviterId"></param>
    /// <returns></returns>
    Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req, string inviterId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<GetAllEventsResponse> GetAllEvents();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task<StatisticViewModel> GetEventStatistic(Guid ownerId);
    Task<GetByIdResponse> GetEventById(Guid id);
}
