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
}
