namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    public class GetEventsByDateResponse : ServiceResponseBase
    {

        public List<EventViewModel> Events { get; set; }

        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
