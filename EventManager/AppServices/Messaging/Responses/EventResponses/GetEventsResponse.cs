namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class GetEventsResponse : ServiceResponseBase
{
    public List<EventViewModel> Events { get; set; }

    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
