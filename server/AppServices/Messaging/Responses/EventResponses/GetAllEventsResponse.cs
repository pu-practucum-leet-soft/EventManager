namespace EventManager.AppServices.Messaging.Responses.EventResponses;

using EventManager.Data.Entities;
using EventManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/// <summary>
/// 
/// </summary>
public class EventSummary
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = default!;
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
    public string? Location { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid OwnerUserId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EventStatus Status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}

/// <summary>
/// 
/// </summary>
public class GetAllEventsResponse
{
    /// <summary>
    /// 
    /// </summary>
    public List<EventSummary> Events { get; set; } = new();
}
