using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Създава нов екземпляр на <see cref="HomeController"/>.
        /// </summary>
        /// <param name="logger">Логер за записване на системни съобщения.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Връща базово приветствено съобщение, показващо,
        /// че Event Manager API-то е стартирано успешно.
        /// </summary>
        /// <returns>Текстов отговор със статус 200 OK.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Event Manager API");
        }
    }
}
