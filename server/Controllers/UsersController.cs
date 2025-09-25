using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        private readonly IJwtHelper _jwtHelper;

        public UsersController(IUsersService service, IJwtHelper jwtHelper)
        {
            _service = service;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Създаване на нов потребител
        /// </summary>
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

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var res = await _service.LoginAsync(request);
            return res.StatusCode == BusinessStatusCodeEnum.Success ? Ok(res) : Unauthorized(res);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh() // чете refresh cookie вътре в service
        {
            var res = await _jwtHelper.RenewRefreshToken();
            return res.StatusCode == BusinessStatusCodeEnum.Success ? Ok(res) : Unauthorized(res);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("me")]
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh-token"];
            if (refreshToken == null) return Unauthorized();
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            var refreshTokenRenual = await _jwtHelper.RenewRefreshToken();
            // върни новото cookie
            Response.Cookies.Append("refresh-token", refreshTokenRenual.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshTokenRenual.TokenExpiryTime
            });

            return Ok(new { token = refreshTokenRenual });
        }

    }
}