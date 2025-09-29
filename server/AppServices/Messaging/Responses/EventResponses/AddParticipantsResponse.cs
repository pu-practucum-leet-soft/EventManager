namespace EventManager.AppServices.Messaging.Responses.EventResponses;

/// <summary> 
/// The response containing the number of invited participants.
/// </summary>
public class AddParticipantsResponse
{
    /// <summary>
    /// An integer that will contain the invited participants.
    /// </summary>
    public int Added { get; set; }
}
