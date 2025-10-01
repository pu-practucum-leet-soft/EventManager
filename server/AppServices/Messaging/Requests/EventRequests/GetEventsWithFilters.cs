namespace EventManager.AppServices.Messaging.Requests.EventRequests;

/// <summary>
/// 
/// </summary>
public class GetEventsWithFiltersRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Location { get; set; }
}
