namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class GetAllEventsResponse : ServiceResponseBase
{
    public List<EventViewModel> Events { get; set; } 
}
