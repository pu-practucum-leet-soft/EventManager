using EventManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities;

public class Event : BaseEntity
{
    [Required]
    [MaxLength(200)]
    [Column("EventTitle")]
    public string Title { get; set; } = default!;
    [MaxLength(400)]
    public string? Location { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    public bool Public { get; set; } = true;

    [Required]
    public Guid OwnerUserId { get; set; }

    [ForeignKey(nameof(OwnerUserId))]
    public User? OwnerUser { get; set; }

    public EventStatus eventStatus { get; set; }

    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}


