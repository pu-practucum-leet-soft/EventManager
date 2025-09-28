using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Responses.HomeResponses;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _service;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHomeService service, ILogger<HomeController> logger)
        {
            _service = service;
            _logger = logger;
        }

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
