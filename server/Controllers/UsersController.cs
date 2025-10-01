namespace EventManager.Controllers
{
    using EventManager.AppServices.Interfaces;
    using EventManager.AppServices.Messaging;
    using EventManager.AppServices.Messaging.Requests.UserRequests;
    using EventManager.AppServices.Messaging.Responses.UserResponses;
    using EventManager.Data.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    /// <summary>
    /// Контролер за управление на потребители.
    /// Предоставя функционалности за регистрация, вход, изход, освежаване на токени и достъп до информация за текущия потребител.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        private readonly IJwtHelper _jwtHelper;

        /// <summary>
        /// Инициализира нов контролер за работа с потребители.
        /// </summary>
        /// <param name="service">Сървис за бизнес логиката, свързана с потребители.</param>
        /// <param name="jwtHelper">Помощен клас за работа с JWT токени.</param>
        public UsersController(IUsersService service, IJwtHelper jwtHelper)
        {
            _service = service;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Създава нов потребител в системата.
        /// </summary>
        /// <param name="model">Модел с данни за създаване на нов потребител.</param>
        /// <returns>Информация за резултата от създаването.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var response = await _service.SaveAsync(new CreateUserRequest(model));

            if (response.StatusCode == BusinessStatusCodeEnum.Conflict)
                return BadRequest(new { message = "Вече съществува потребител с този имейл." });

            if (response.StatusCode == BusinessStatusCodeEnum.InternalServerError)
                return StatusCode(500, new { message = "Възникна вътрешна грешка." });

            return Ok(response);
        }

        /// <summary>
        /// Вход на потребител със зададени имейл и парола.
        /// </summary>
        /// <param name="request">Данни за вход - имейл и парола.</param>
        /// <returns>JWT токен и информация за потребителя.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] AppServices.Messaging.Requests.UserRequests.LoginRequest request)
        {
            var res = await _service.LoginAsync(request);
            if (res.StatusCode != BusinessStatusCodeEnum.Success) return Unauthorized(res);

            return Ok(new
            {
                token = res.Token,
                expiresAt = res.TokenExpiryTime,
                user = new { res.UserName, res.Email, res.Role }
            });
        }

        /// <summary>
        /// Обновява JWT токена чрез подаден refresh токен.
        /// </summary>
        /// <param name="req">Модел с refresh токен.</param>
        /// <returns>Нов JWT токен или грешка при невалиден refresh токен.</returns>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var res = await _jwtHelper.RenewRefreshToken(req);
            return res.StatusCode == BusinessStatusCodeEnum.Success ? Ok(res) : Unauthorized(res);
        }

        /// <summary>
        /// Изход на текущия потребител и анулиране на неговите токени.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return Ok();
        }

        /// <summary>
        /// Връща информация за текущо автентикирания потребител.
        /// </summary>
        /// <returns>Идентификатор и роля на потребителя.</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                UserId = userId,
                Role = role
            });
        }

        /// <summary>
        /// Намира потребител по имейл.
        /// </summary>
        /// <param name="email">Имейл на потребителя.</param>
        /// <returns>Идентификатор на намерения потребител или грешка 404.</returns>
        [HttpGet("by-email")]
        [ProducesResponseType(typeof(UserIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var result = await _service.GetUserIdByEmailAsync(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
