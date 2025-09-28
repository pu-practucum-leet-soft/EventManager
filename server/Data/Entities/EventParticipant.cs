namespace EventManager.Data.Entities;

using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// 
/// </summary>
public class EventParticipant
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Guid EventId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [ForeignKey(nameof(EventId))]
    public Event? Event { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Guid InviteeId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [ForeignKey(nameof(InviteeId))]
    public User? Invitee { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Guid InviterId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [ForeignKey(nameof(InviterId))]
    public User? Inviter { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public InviteStatus Status { get; set; }
}
