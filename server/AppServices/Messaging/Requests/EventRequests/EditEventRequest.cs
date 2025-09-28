namespace EventManager.AppServices.Messaging.Requests.EventRequests;

using EventManager.Data.Enums;

/// <summary>
/// 
/// </summary>
public class EditEventRequest
{
    /// <summary>
    /// 
    /// </summary>
    public Guid EventId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid ActorUserId { get; set; } // user performing the edit (must be owner)
    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; set; }
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
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EventStatus Status{ get; set; }
}
