namespace EventManager.AppServices.Messaging.Requests.EventRequests;

/// <summary>
/// 
/// </summary>
public class AddParticipantsRequest
{
    /// <summary>
    /// 
    /// </summary>
    public Guid EventId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid ActorUserId { get; set; } // must be owner
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Guid> UserIds { get; set; } = Array.Empty<Guid>();
}
