namespace EventManager.Controllers;

using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// API контролер за управление на събития.
/// Позволява създаване, редактиране, извличане и статистика на събития,
/// както и управление на участници.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    /// <summary>
    /// Инициализира нов екземпляр на контролера за събития.
    /// </summary>
    /// <param name="service">Сървис за работа със събития.</param>
    public EventsController(IEventService service) => _service = service;

    /// <summary>
    /// Създава ново събитие за текущо автентикирания потребител.
    /// </summary>
    /// <param name="req">Данните за събитието.</param>
    /// <returns>Резултат от операцията по създаване.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var res = await _service.CreateEvent(req, userId!);
        return Ok(res);
    }

    /// <summary>
    /// Връща информация за събитие по неговото Id.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventById([FromRoute] string id)
    {
        Console.WriteLine(id);
        if (!Guid.TryParse(id, out var eventId)) return BadRequest("Invalid event ID.");
        var res = await _service.GetEventById(eventId);
        if (res == null) return NotFound();
        return Ok(res);
    }

    /// <summary>
    /// Редактира съществуващо събитие.
    /// </summary>
    [HttpPut("{eventId}")]
    [Authorize]
    [ProducesResponseType(typeof(EditEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit([FromRoute] string eventId, [FromBody] EditEventRequest req)
    {
        Console.WriteLine(eventId);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized();
        }
        if (!Guid.TryParse(eventId, out var eventIdGuid)) return BadRequest("Invalid event ID.");

        var res = await _service.EditEvent(eventIdGuid, userGuid, req);
        return Ok(res);
    }

    /// <summary>
    /// Cancel existing event.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CancelEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        if(!Guid.TryParse(userId, out var userGuid))
        {
            return Unauthorized();
        }

        var res = await _service.CancelEvent(id, userGuid);
        return Ok(res);
    }

    /// <summary>
    /// Добавя участници към дадено събитие.
    /// </summary>
    /// <param name="id">Id на събитието.</param>
    /// <param name="req">Списък с участници за добавяне.</param>
    /// <returns>Резултат от операцията.</returns>
    [HttpPost("{id:guid}/participants")]
    [Authorize]
    [ProducesResponseType(typeof(AddParticipantsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddParticipants(Guid id, [FromBody] AddParticipantsRequest req)
    {
        req.EventId = id;
        var inviterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var res = await _service.AddParticipants(req, inviterId!);
        return Ok(res);
    }    

    /// <summary>
    /// Връща списък с всички събития в системата.
    /// </summary>
    /// <returns>Колекция от събития.</returns>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(IEnumerable<GetAllEventsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var res = await _service.GetAllEvents();
        return Ok(res);
    }

    /// <summary>
    /// Връща статистика за събитията на текущия потребител.
    /// </summary>
    /// <returns>Обект със статистическа информация за събитията.</returns>
    [HttpGet("statistic")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<StatisticViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStatistic()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var res = await _service.GetEventStatistic(userId);
        return Ok(res);
    }

    /// <summary>
    /// Връща списък с филтрирани събития (Events).
    /// </summary>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(IEnumerable<GetAllEventsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> GetEventsWithFilters([FromQuery] GetEventsWithFiltersRequest req)
    {
        var res = await _service.GetEventsWithFilters(req);
        return Ok(res);
    }
}
