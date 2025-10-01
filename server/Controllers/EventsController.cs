using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManager.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public EventsController(IEventService service) => _service = service;

    /// <summary>
    /// Create a new event.
    /// </summary>
    [HttpPost]
    [Authorize] // owner must be authenticated
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
    /// Get event details by ID.
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
    /// Edit existing event.
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
    /// Add participants to event.
    /// </summary>
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
    /// Get all events.
    /// </summary>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(IEnumerable<GetAllEventsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var res = await _service.GetAllEvents();
        return Ok(res);
    }

    /// <summary>
    /// Get all events.
    /// </summary>
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
    /// Get events with filters.
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



