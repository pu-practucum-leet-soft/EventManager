namespace EventManager.AppServices.Messaging.Requests.EventRequests;

using EventManager.Data.Enums;

/// <summary>
/// Заявка за редактиране на съществуващо събитие.
/// Използва се при промяна на детайлите на събитие от неговия собственик.
/// </summary>
public class EditEventRequest
{
    /// <summary>
    /// Ново заглавие/име на събитието (по желание).
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Ново местоположение за събитието (по желание).
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Ново описание или бележки към събитието (по желание).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Актуализирана начална дата и час на събитието.
    /// </summary>
    public DateTime StartDate { get; set; }
}
