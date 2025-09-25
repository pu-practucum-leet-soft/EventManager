namespace EventManager.AppServices.Messaging.Requests.EventRequests;

public class GetEventsRequest : ServiceRequestBase
{
    public Guid ActingUserId { get; set; }
    public bool IsParticipant { get; set; }
    public bool IsAdmin { get; set; }         
    public bool Owned { get; set; } = false;     
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;


}
