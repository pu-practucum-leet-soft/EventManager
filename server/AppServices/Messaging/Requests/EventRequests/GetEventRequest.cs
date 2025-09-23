namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class GetEventRequest
{
    public Guid EventId { get; set; }
       
    public GetEventRequest(Guid eventId)
    {
        EventId = eventId;
    }

}
