using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;

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

            Console.WriteLine(model);

            return Ok(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            return Ok(await _service.LoginAsync(request));
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var email = User.FindFirstValue(ClaimTypes.Name); // ако в токена Name е email

            if (userId == null)
                return Unauthorized(new { message = "Missing or invalid token." });

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


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "API is working!" });
        }

    }
}