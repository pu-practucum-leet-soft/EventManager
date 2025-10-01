using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Data.Entities;

/// <summary>
/// Представлява събитие в системата.
/// Включва основна информация като заглавие, описание, дата, локация,
/// собственик и участници.
/// </summary>
[PrimaryKey(nameof(Id))]
[Table("Events")]
public class Event
{
    /// <summary>
    /// Уникален идентификатор на събитието (генериран от базата).
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Заглавие на събитието. Задължително поле с максимум 200 символа.
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column("EventTitle")]
    public string? Title { get; set; } = default!;

    /// <summary>
    /// Подробно описание на събитието. Опционално поле с максимум 2000 символа.
    /// </summary>
    [MaxLength(2000)]
    [Column("EventDescription")]
    public string? Description { get; set; }

    /// <summary>
    /// Дата и час на началото на събитието.
    /// </summary>
    [Column("EventStartDate")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Локация, където ще се проведе събитието (до 50 символа).
    /// </summary>
    [MaxLength(50)]
    public string? Location { get; set; }

    /// <summary>
    /// Идентификатор на потребителя, който е създател на събитието.
    /// </summary>
    [Required]
    public Guid OwnerUserId { get; set; }

    /// <summary>
    /// Навигационно свойство към собственика (потребител) на събитието.
    /// </summary>
    [ForeignKey(nameof(OwnerUserId))]
    public User? OwnerUser { get; set; }

    /// <summary>
    /// Текущият статус на събитието (Active, Cancelled, Past, Archived).
    /// </summary>
    public EventStatus Status { get; set; }

    /// <summary>
    /// Списък с участниците, които са поканени за събитието.
    /// </summary>
    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}
