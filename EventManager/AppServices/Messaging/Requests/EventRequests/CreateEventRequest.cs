

using System.Text.Json.Serialization;

namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class CreateEventRequest : ServiceRequestBase
{
    public EventModel Event { get; set; }

    [JsonConstructor] 
    public CreateEventRequest(EventModel Event) 
    {
        this.Event = Event;
    }
}
