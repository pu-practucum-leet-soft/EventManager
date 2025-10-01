namespace EventManager.AppServices.Messaging.Responses.EventResponses;

using EventManager.Data.Entities;
using EventManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Кратка информация за събитие, използвана при връщане на обобщен списък.
/// </summary>
public class EventSummary
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
    /// Кратко описание на събитието.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Дата и час на започване на събитието.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Местоположение на събитието.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Идентификатор на потребителя – създател на събитието.
    /// </summary>
    public Guid OwnerUserId { get; set; }

    /// <summary>
    /// Текущ статус на събитието.
    /// </summary>
    public EventStatus Status { get; set; }

    /// <summary>
    /// Участниците, поканени или добавени към събитието.
    /// </summary>
    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}

/// <summary>
/// Отговор, съдържащ списък от всички налични събития.
/// </summary>
public class GetAllEventsResponse
{
    /// <summary>
    /// Колекция от събития във вид на обобщени модели (<see cref="EventSummary"/>).
    /// </summary>
    public List<EventSummary> Events { get; set; } = new();
}
