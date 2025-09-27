namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class AddParticipantsResponse : ServiceResponseBase
{
    public int Added { get; set; }
    public int SkippedExisting { get; set; }
    public int SkippedMissingUsers { get; set; }
}
