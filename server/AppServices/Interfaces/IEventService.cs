namespace EventManager.AppServices.Interfaces;

using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;

/// <summary>
/// Интерфейс за услугата за управление на събития.
/// Съдържа бизнес логиката за създаване, редактиране,
/// извличане и статистика на събития.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Създава ново събитие за конкретен потребител.
    /// </summary>
    /// <param name="req">Заявка със свойствата на събитието.</param>
    /// <param name="userId">Идентификаторът на потребителя, който е създател на събитието.</param>
    /// <returns>Резултат от операцията със статус и информация за създаденото събитие.</returns>
    Task<CreateEventResponse> CreateEvent(CreateEventRequest req, string userId);

    /// <summary>
    /// Извлича детайли за събитие по неговия идентификатор.
    /// </summary>
    /// <param name="req">Заявка, съдържаща идентификатор на събитието.</param>
    /// <returns>Модел с информация за събитието.</returns>
    Task<EventViewModel> GetEvent(GetEventRequest req);

    /// <summary>
    /// Редактира вече съществуващо събитие.
    /// </summary>
    /// <param name="eventId">Идентификатор на събитието за редакция.</param>
    /// <param name="userId">Идентификатор на потребителя, който прави редакция.</param>
    /// <param name="req">Заявка с новите данни за събитието.</param>
    /// <returns>Резултат дали редакцията е успешна.</returns>
    Task<EditEventResponse> EditEvent(Guid eventId, Guid userId, EditEventRequest req);

    /// <summary>
    /// Маркира събитие като отказано.
    /// </summary>
    /// <param name="eventId">Уникален идентификатор на събитието, което ще бъде отказано.</param>
    /// <param name="userId">Идентификатор на потребителя, който отказва събитето.</param>
    /// <returns>Резултат с информация за операцията.</returns>
    Task<CancelEventResponse> CancelEvent(Guid eventId, Guid userId);

    /// <summary>
    /// Добавя участници към събитие.
    /// </summary>
    /// <param name="req">Заявка с информация за събитието и потребителите, които да се добавят.</param>
    /// <param name="inviterId">Идентификатор на потребителя, който добавя участниците.</param>
    /// <returns>Резултат от операцията с информация за броя добавени участници.</returns>
    Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req, string inviterId);

    /// <summary>
    /// Извлича списък с всички събития.
    /// </summary>
    /// <returns>Отговор със списък от всички събития.</returns>
    Task<GetAllEventsResponse> GetAllEvents();

    /// <summary>
    /// Извлича статистика за събитията на даден собственик.
    /// </summary>
    /// <param name="ownerId">Идентификатор на собственика на събитията.</param>
    /// <returns>Статистически данни за събитията на собственика.</returns>
    Task<StatisticViewModel> GetEventStatistic(Guid ownerId);

    /// <summary>
    /// Връща събитие по зададен идентификатор.
    /// </summary>
    /// <param name="id">Уникален идентификатор на събитието.</param>
    /// <returns><see cref="GetByIdResponse"/> със съдържание на събитието или статус NotFound при липса.</returns>
    Task<GetByIdResponse> GetEventById(Guid id);

    /// <summary>
    /// Връща списък от събития, като прилага филтри по заглавие, дата или локация.
    /// </summary>
    /// <param name="req">Заявка, съдържаща филтри за заглавие, начална дата и местоположение.</param>
    /// <returns><see cref="GetAllEventsResponse"/> със списък от събития, отговарящи на филтрите.</returns>
    Task<GetAllEventsResponse> GetEventsWithFilters(GetEventsWithFiltersRequest req);
}
