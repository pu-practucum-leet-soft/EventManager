using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;
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
        var res = await _service.CreateEvent(req);
        return Ok(res);
    }

    /// <summary>
    /// Get event details by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EventViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var res = await _service.GetEvent(new GetEventRequest { EventId = id });
        if (res == null) return NotFound();
        return Ok(res);
    }

    /// <summary>
    /// Edit existing event.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(EditEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditEventRequest req)
    {
        req.EventId = id;
        var res = await _service.EditEvent(req);
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
        var res = await _service.AddParticipants(req);
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
        var res = await _service.GetAllEvents();
        return Ok(res);
    }
}

