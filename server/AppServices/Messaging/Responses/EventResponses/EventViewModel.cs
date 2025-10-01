using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.AppServices.Messaging.Responses.EventResponses;

/// <summary>
/// Модел за представяне на събитие при връщане на отговор от услугите.
/// </summary>
public class EventViewModel
{
    /// <summary>
    /// Уникален идентификатор на събитието.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Заглавие на събитието.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Описание на събитието.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Дата и час на стартиране на събитието.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Местоположение на събитието.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Идентификатор на потребителя, създал събитието.
    /// </summary>
    public Guid OwnerUserId { get; set; }

    /// <summary>
    /// Текущ статус на събитието (активно, отменено, архивирано).
    /// </summary>
    public EventStatus Status { get; set; }

    /// <summary>
    /// Списък с участниците в събитието.
    /// </summary>
    public ICollection<EventParticipant>? Participants { get; set; }
}
