using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;
    public EventsController(IEventService service) => _service = service;

    [HttpPost]
    [Authorize] // owner must be authenticated
    public async Task<IActionResult> Create([FromBody] CreateEventRequest req)
    {
        var res = await _service.CreateEvent(req);
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var res = await _service.GetEvent(new GetEventRequest { EventId = id });
        return Ok(res);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditEventRequest req)
    {
        req.EventId = id;
        var res = await _service.EditEvent(req);
        return Ok(res);
    }

    [HttpPost("{id:guid}/participants")]
    [Authorize]
    public async Task<IActionResult> AddParticipants(Guid id, [FromBody] AddParticipantsRequest req)
    {
        req.EventId = id;
        var res = await _service.AddParticipants(req);
        return Ok(res);
    }

    [HttpGet]
    [Route("/get-all")]
    public async Task<IActionResult> GetAll()
    {
        var res = await _service.GetAllEvents();
        return Ok(res);
    }
}
