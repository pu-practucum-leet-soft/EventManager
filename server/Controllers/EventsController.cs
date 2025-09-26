using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventManager.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;
    public EventsController(IEventService service) => _service = service;


    //CreateEvent 
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEventAsync([FromBody] CreateEventRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var ownerId))
            return Unauthorized(new ServiceResponseError { Message = "Invalid identity." });

        var res = await _service.SaveEventAsync(req, ownerId);

        return Ok(res);
    }



    //Edit 
    [Authorize]
    [HttpPost("edit/{id:guid}")]
    [ProducesResponseType(typeof(EditEventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditChangesEventAsync([FromBody] EventModel _event)

    {
        // userId от токена
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var actingUserId))
            return Unauthorized(new ServiceResponseError
            {
                StatusCode = BusinessStatusCodeEnum.Unauthorized,
                Message = "Липсва валидна идентичност."
            });

        var res = await _service.UpdateEventAsync(new(_event), actingUserId);


        return Ok(res);

    }





    // GetEvents
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(GetEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventsAsync(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(meStr, out var me);

        var req = new GetEventsRequest
        {
            ActingUserId = me,
            Page = page,
            PageSize = pageSize
        };

        var res = await _service.LoadEventsAsync(req);
        return Ok(res);

    }


    // GetEvents
    [Authorize]
    [HttpGet("title")]
    [ProducesResponseType(typeof(GetEventsByTitleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventsByTitleAsync(
    [FromQuery] string title,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(meStr, out var me);
        
        var req = new GetEventsByTitleRequests
        {
            ActingUserId = me,
            EventTitle = title,
            Page = page,
            PageSize = pageSize
        };

        var res = await _service.LoadEventsByTitleAsync(req); 
        return Ok(res);

    }



    // GetMyOwnedEvents
    [Authorize]
    [HttpGet("my-events")]
    [ProducesResponseType(typeof(GetEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMyOwnedEventsAsync(
    [FromQuery] bool owned = true,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(meStr, out var me)) return Unauthorized();

        var req = new GetEventsRequest
        {
            Owned = owned,
            ActingUserId = me,
            Page = page,
            PageSize = pageSize
        };

        var res = await _service.LoadMyOwnedEventsAsync(req);
        return Ok(res);

    }





    // GetMyEventsPraticipate
    [Authorize]
    [HttpGet("invited")]
    [ProducesResponseType(typeof(GetEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMyEventsPraticipateAsync(
    [FromQuery] bool _IsParticipant = true,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(meStr, out var me)) return Unauthorized();

        var req = new GetEventsRequest
        {
            ActingUserId = me,
            IsParticipant = _IsParticipant,
            Page = page,
            PageSize = pageSize
        };

        var res = await _service.LoadEventsAsync(req);
        return Ok(res);

    }

    

    // GetEventById
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetEventByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventByIdAsync([FromRoute] Guid id)

    {

             var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(meStr, out var me))
            return Unauthorized();


            var res = await _service.LoadEventByIdAsync(new GetEventByIdRequest(id), me);
            return Ok(res);
    }



    // AddParticipants
    [Authorize]
    [HttpPost("{id:guid}/join")]
    [ProducesResponseType(typeof(AddParticipantsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddParticipantsAsync(
    [FromRoute] Guid id,
    [FromBody] List<Guid> userIds)
    {
        // userId от токена
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var actingUserId))
            return Unauthorized(new ServiceResponseError
            {
                StatusCode = BusinessStatusCodeEnum.Unauthorized,
                Message = "Липсва валидна идентичност."
            });

        var res = await _service.SaveParticipantsAsync(new AddParticipantsRequest
        {
            EventId = id,
            UserIds = userIds ?? new List<Guid>()
        }, actingUserId);

        return Ok(res);
       
    }






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
