using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    public class GetEventsByLocationRequests : ServiceRequestBase
    {
        public Guid ActingUserId { get; set; }

        public string EventLocation { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
