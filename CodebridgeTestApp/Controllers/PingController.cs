using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTestApp.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
    }
}
