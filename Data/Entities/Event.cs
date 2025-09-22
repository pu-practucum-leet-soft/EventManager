using System.ComponentModel.DataAnnotations;

namespace EventManager.Data.Entities;

public class Event : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [MaxLength(400)]
    public string? Location { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [Required]
    public Guid OwnerUserId { get; set; }
    public User? OwnerUser { get; set; }

    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}
