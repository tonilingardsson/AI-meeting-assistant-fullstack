using Microsoft.AspNetCore.Mvc;

namespace AI_Meeting_Assistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "API is running" });
        }
    }
}