namespace EventManager.AppServices.Messaging.Requests.EventRequests;

/// <summary>
/// 
/// </summary>
public class CreateEventRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// 
    /// </summary>
    public string? Location { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid OwnerUserId { get; set; }
}
