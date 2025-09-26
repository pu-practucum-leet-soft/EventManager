using EventManager.AppServices.Messaging.Responses.EventResponses;

namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    public class GetEventsByTitleResponse : ServiceResponseBase
    {
        public List<EventViewModel> Events { get; set; }

        public string Title { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
