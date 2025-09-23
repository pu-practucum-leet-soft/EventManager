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
    public IActionResult Create([FromBody] CreateEventRequest req)
    {
        var res = _service.CreateEvent(req);
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEventById(Guid id)
    {
        var res = _service.GetEvent(new GetEventRequest { EventId = id });
        return Ok(res);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public IActionResult Edit(Guid id, [FromBody] EditEventRequest req)
    {
        req.EventId = id;
        var res = _service.EditEvent(req);
        return Ok(res);
    }

    [HttpPost("{id:guid}/participants")]
    [Authorize]
    public IActionResult AddParticipants(Guid id, [FromBody] AddParticipantsRequest req)
    {
        req.EventId = id;
        var res = _service.AddParticipants(req);
        return Ok(res);
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] Guid? ownerUserId)
    {
        var res = _service.GetAllEvents(new GetAllEventsRequest { OwnerUserId = ownerUserId });
        return Ok(res);
    }
}
