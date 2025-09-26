namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    public class GetEventsByDateRequests : ServiceRequestBase
    {
        public Guid ActingUserId { get; set; }

        public DateTime EventDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
