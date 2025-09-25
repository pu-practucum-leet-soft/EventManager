namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    public class GetEventByIdResponse : ServiceResponseBase
    {
        public EventViewModel Event { get; set; }

    }
}
