

namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class CreateEventRequest : ServiceRequestBase
{
    public EventModel Event { get; set; }

    public CreateEventRequest(EventModel _event)
    {
        Event = _event;
    }
}
