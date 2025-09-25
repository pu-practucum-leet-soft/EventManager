namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class EditEventRequest : ServiceRequestBase
{
    public Guid ActorUserId { get; set; } // user performing the edit (must be owner)
    public EventModel Event { get; set; }


    public EditEventRequest(EventModel _event)
    {
        Event = _event;
    }
}
