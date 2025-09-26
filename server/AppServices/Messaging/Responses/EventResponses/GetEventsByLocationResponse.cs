namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    public class GetEventsByLocationResponse : ServiceResponseBase
    {

        public List<EventViewModel> Events { get; set; }

        public string Location { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }


    }
}
