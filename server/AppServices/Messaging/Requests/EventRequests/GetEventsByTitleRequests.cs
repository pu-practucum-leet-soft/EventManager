namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    public class GetEventsByTitleRequests : ServiceRequestBase
    {
        public Guid ActingUserId { get; set; }

        public string EventTitle  { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
