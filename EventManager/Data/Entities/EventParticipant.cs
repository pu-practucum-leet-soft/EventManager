using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities;


public class EventParticipant : BaseEntity
{

    [Required]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public Event? Event { get; set; }

    [Required]
    public Guid InviteeId { get; set; }

    [ForeignKey(nameof(InviteeId))]
    public User? Invitee { get; set; }

    [Required]
    public Guid InviterId { get; set; }

    [ForeignKey(nameof(InviterId))]
    public User? Inviter { get; set; }

    [Required]
    public InviteStatus inviteStatus { get; set; }


}
