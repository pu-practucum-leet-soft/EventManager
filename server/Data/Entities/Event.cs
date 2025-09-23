using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities;

[PrimaryKey(nameof(Id))]
[Table("Events")]
public class Event
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("EventTitle")]
    public string Title { get; set; } = default!;

    [MaxLength(2000)]
    [Column("EventDescription")]
    public string? Description { get; set; }

    [Column("EventName")]
    public DateTime? StartDate { get; set; }

    [MaxLength(50)]
    public string? Location { get; set; }

    [Required]
    public Guid OwnerUserId { get; set; }

    [ForeignKey(nameof(OwnerUserId))]
    public User? OwnerUser { get; set; }

    public EventStatus Status { get; set; }

    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();

}


