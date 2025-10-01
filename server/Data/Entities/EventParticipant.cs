namespace EventManager.Data.Entities;

using EventManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представлява връзката между събитие и потребители (покани).
/// Всеки запис описва покана на потребител за определено събитие,
/// кой го е поканил и текущия статус на поканата.
/// </summary>
public class EventParticipant
{
    /// <summary>
    /// Уникален идентификатор на записа.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор на събитието, към което е изпратена поканата.
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Навигационно свойство към събитието.
    /// </summary>
    [ForeignKey(nameof(EventId))]
    public Event? Event { get; set; }

    /// <summary>
    /// Идентификатор на потребителя, който е поканен (получател на поканата).
    /// </summary>
    [Required]
    public Guid InviteeId { get; set; }

    /// <summary>
    /// Навигационно свойство към поканения потребител.
    /// </summary>
    [ForeignKey(nameof(InviteeId))]
    public User? Invitee { get; set; }

    /// <summary>
    /// Идентификатор на потребителя, който е изпратил поканата.
    /// </summary>
    [Required]
    public Guid InviterId { get; set; }

    /// <summary>
    /// Навигационно свойство към потребителя, изпратил поканата.
    /// </summary>
    [ForeignKey(nameof(InviterId))]
    public User? Inviter { get; set; }

    /// <summary>
    /// Статус на поканата (например: Invited, Accepted, Declined).
    /// </summary>
    [Required]
    public InviteStatus Status { get; set; }
}
