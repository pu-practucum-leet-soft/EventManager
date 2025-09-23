using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;
    public EventsController(IEventService service) => _service = service;


    //CreateEvent 
    [HttpPost]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEventAsync([FromBody] EventModel _event) => Ok(await _service.SaveEventAsync(new(_event)));

    /*
    // GetAllEvents
    [HttpGet]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEventAsync([FromBody] EventModel _event) => Ok(await _service.CreateEventAsync(new(_event)));
    */


    // GetEventById
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventByIdAsync([FromBody] Guid id) => Ok(await _service.LoadEventByIdAsync(new(id)));
    

    /*
    // GetAllEvents for owers
    [HttpGet]
    public IActionResult GetAll([FromQuery] Guid? ownerUserId)
    {
        var res = _service.GetAllEvents(new GetAllEventsRequest { OwnerUserId = ownerUserId });
        return Ok(res);
    }
    */


    /* [HttpGet("{id:guid}")]
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

    */



}
