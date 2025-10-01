using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Responses.HomeResponses;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventManager.Controllers
{
    /// <summary>
    /// Контролер за началната страница на API-то.
    /// Използва се основно за тестове и проверка дали API-то работи.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _service;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Създава нов екземпляр на <see cref="HomeController"/>.
        /// </summary>
        /// <param name="service">Услуга, която дава контрол над свойствата на <i>home</i>.</param>
        /// <param name="logger">Логер за записване на системни съобщения.</param>
        public HomeController(IHomeService service, ILogger<HomeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Връща базово приветствено съобщение, показващо,
        /// че Event Manager API-то е стартирано успешно.
        /// </summary>
        /// <returns>Текстов отговор със статус 200 OK.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var me)) return Unauthorized();
            return _service.GetHome(me).Result is GetHomeResponse res ? Ok(res) : NotFound();
        }
    }
}
