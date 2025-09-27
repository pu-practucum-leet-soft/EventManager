namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class GetEventByIdRequest : ServiceRequestBase
{
    public Guid EventId { get; set; }
       
    public GetEventByIdRequest(Guid eventId)
    {
        EventId = eventId;
    }

}
