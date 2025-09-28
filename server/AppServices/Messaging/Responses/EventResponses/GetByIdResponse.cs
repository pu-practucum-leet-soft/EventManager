namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class GetByIdResponse : ServiceResponseBase
{
    public EventViewModel Event { get; set; } = default!;
}