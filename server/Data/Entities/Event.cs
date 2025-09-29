using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities;
/// <summary>
/// 
/// </summary>
[PrimaryKey(nameof(Id))]
[Table("Events")]
public class Event
{
    /// <summary>
    /// 
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column("EventTitle")]
    public string? Title { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(2000)]
    [Column("EventDescription")]
    public string? Description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Column("EventStartDate")]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [MaxLength(50)]
    public string? Location { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Guid OwnerUserId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [ForeignKey(nameof(OwnerUserId))]
    public User? OwnerUser { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EventStatus Status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();

}


