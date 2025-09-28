using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Event Manager API");
        }
    }
}
