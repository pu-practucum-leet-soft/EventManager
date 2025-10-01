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
    /// <summary>
    /// API контролер за управление на покани към събития.
    /// Предоставя функционалности за извличане, създаване, приемане, отказване и отписване от събития.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesController : ControllerBase
    {
        private readonly IInvitesService _service;

        /// <summary>
        /// Конструктор за инициализация на <see cref="InvitesController"/>.
        /// </summary>
        /// <param name="service">Сървис за управление на покани.</param>
        public InvitesController(IInvitesService service) => _service = service;

        /// <summary>
        /// Връща всички покани на текущо автентикирания потребител (входящи и изходящи).
        /// </summary>
        /// <returns><see cref="GetInvitesAllResponse"/> със списък от покани.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetInvitesAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var meStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(meStr, out var me)) return Unauthorized();

            var res = await _service.LoadAllInvitesAsync(me);
            return Ok(res);
        }

        /// <summary>
        /// Връща входящите покани за текущо автентикирания потребител.
        /// </summary>
        /// <param name="page">Номер на страница (по подразбиране 1).</param>
        /// <param name="pageSize">Брой резултати на страница (по подразбиране 20).</param>
        /// <returns><see cref="GetInvitesIncomingResponse"/> със списък от входящи покани.</returns>
        [Authorize]
        [HttpGet("incoming")]
        [ProducesResponseType(typeof(GetInvitesIncomingResponse), StatusCodes.Status200OK)]
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


        /// <summary>
        /// Връща изходящите покани за текущо автентикирания потребител.
        /// </summary>
        /// <param name="page">Номер на страница (по подразбиране 1).</param>
        /// <param name="pageSize">Брой резултати на страница (по подразбиране 20).</param>
        /// <returns><see cref="GetInvitesOutcomingResponse"/> със списък от изходящи покани.</returns>
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

        /// <summary>
        /// Приема покана за участие в събитие от името на текущия потребител.
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <returns>HTTP 200 със статус на операцията или грешка при неуспех.</returns>
        [Authorize]
        [HttpPost("{eventId:guid}/accept")]
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



        /// <summary>
        /// Отказва покана за участие в събитие от името на текущия потребител.
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <returns>HTTP 200 със статус на операцията или грешка при неуспех.</returns>
        [Authorize]
        [HttpPost("{eventId:guid}/decline")]
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

        /// <summary>
        /// Създава нова покана за събитие от името на текущия потребител.
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <param name="req">Заявка с имейл на поканения потребител.</param>
        /// <returns>HTTP 200 при успех или грешка (404, 400, 409).</returns>
        [Authorize]
        [HttpPost("{eventId:guid}/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInviteAsync([FromRoute] Guid eventId, [FromBody] CreateInviteRequest req)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var actingUserId))
                return Unauthorized(new ServiceResponseError
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва валиден потребител."
                });

            if (eventId == Guid.Empty)
            {
                return BadRequest(new ServiceResponseError
                {
                    StatusCode = BusinessStatusCodeEnum.BadRequest,
                    Message = "Липсва валидно събитие."
                });
            }

            Console.WriteLine($"Creating invite to event {eventId} from user {actingUserId} to email {req.InviteeEmail}");
            var res = await _service.CreateInviteAsync(eventId, actingUserId, req.InviteeEmail!);

            if (res.StatusCode == BusinessStatusCodeEnum.NotFound)
            {
                return NotFound(res);
            }

            if (res.StatusCode == BusinessStatusCodeEnum.BadRequest)
            {
                return BadRequest(res);
            }

            if (res.StatusCode == BusinessStatusCodeEnum.Conflict)
            {
                return Conflict(res);
            }


            return Ok(res);
        }

        /// <summary>
        /// Отписва текущия потребител от събитие, като отказва всички негови активни покани.
        /// </summary>
        /// <param name="eventId">Идентификатор на събитието.</param>
        /// <returns>HTTP 200 при успех или грешка при неуспех.</returns>
        [Authorize]
        [HttpDelete("{eventId:guid}/unattend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnattendEventAsync([FromRoute] Guid eventId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var actingUserId))
                return Unauthorized(new ServiceResponseError
                {
                    StatusCode = BusinessStatusCodeEnum.Unauthorized,
                    Message = "Липсва валиден потребител."
                });

            var res = await _service.UnattendEventAsync(eventId, actingUserId);

            if (res.StatusCode == BusinessStatusCodeEnum.NotFound)
            {
                return NotFound(res);
            }

            if (res.StatusCode == BusinessStatusCodeEnum.BadRequest)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
    }
}