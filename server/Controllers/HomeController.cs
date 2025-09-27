using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Responses.HomeResponses;
using Microsoft.AspNetCore.Authorization;

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
            // TODO: Process the response from the service properly
            // TODO: Replace with actual user ID from auth context
            return _service.GetHome("test-user-id").Result is GetHomeResponse res ? Ok(res) : NotFound();
        }
    }
}
