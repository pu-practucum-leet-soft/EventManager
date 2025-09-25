using EventManager.AppServices.Implementations;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.InvitesRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.InvitesResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesController : ControllerBase
    {

        private readonly InvitesService _service;

        public InvitesController(InvitesService service) => _service = service;


        [Authorize]
        [HttpGet("incoming")]
        [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIncomingAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(meStr, out var me)) return Unauthorized();

            var res = await _service.LoadInvitesIncomingAsync(new GetInvitesIncomingRequest
            {
                ActingUserId = me,
                Page = page,
                PageSize = pageSize
            });

            return Ok(res);
        }



        [Authorize]
        [HttpGet("outcoming")]
        [ProducesResponseType(typeof(GetInvitesIncomingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOutcomingAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(meStr, out var me)) return Unauthorized();

            var res = await _service.LoadInvitesOutcomingAsync(new GetInvitesOutcomingRequest
            {
                ActingUserId = me,
                Page = page,
                PageSize = pageSize
            });

            return Ok(res);
        }

        
        
        
        
        [Authorize]
        [HttpPost("{id:guid}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptInvitesAsync([FromRoute] Guid eventId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var actingUserId))
                return Unauthorized(new ServiceResponseError
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва валидна идентичност."
                });

            var res = await _service.AcceptInviteAsync(eventId, actingUserId);
            return Ok(res);
        }




        [Authorize]
        [HttpPost("{id:guid}/Declined")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeclineInvitesAsync([FromRoute] Guid eventId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var actingUserId))
                return Unauthorized(new ServiceResponseError
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва валидна идентичност."
                });

            var res = await _service.DeclineInviteAsync(eventId, actingUserId);
            return Ok(res);
        }

    }
}
