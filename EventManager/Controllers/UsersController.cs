using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Създаване на нов потребител
        /// </summary>
        [HttpPost("register")]
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

            Console.WriteLine(model);

            return Ok(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResponse = await _service.LoginAsync(request);

            if (loginResponse.StatusCode != BusinessStatusCodeEnum.Success || string.IsNullOrEmpty(loginResponse.Token))
            {
                return Unauthorized(new { message = "Невалиден имейл или парола." });
            }

            // JWT токен от loginResponse
            var token = loginResponse.Token;

            // Слагаме го в secure cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,             // 🔐 JS не може да го чете
                Secure = true,               // работи само по HTTPS (ако си на localhost може да го махнеш за тест)
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            // Връщаме response-а + info за user-а
            return Ok(new
            {
                loginResponse.Id,
                loginResponse.Email,
                loginResponse.FirstName,
                loginResponse.LastName,
                loginResponse.Role,
                loginResponse.StatusCode,
                message = "Успешен вход!"
            });
        }
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetMe()
        {
            // Четем userId и role от JWT (cookie-то)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Няма активна сесия." });
            }

            var jwtCookie = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(jwtCookie))
            {
                return Unauthorized(new { message = "Cookie not found!" });
            }

            return Ok(new
            {
                Id = userId,
                Email = email,
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

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // изтриваме cookie-то
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "Успешно излизане." });
        }


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "API is working!" });
        }

    }
}