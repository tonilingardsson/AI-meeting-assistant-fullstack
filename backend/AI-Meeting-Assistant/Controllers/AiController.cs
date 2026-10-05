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

        [HttpPost("agenda")]
        public IActionResult CreateAgenda([FromBody] AgendaRequest request)
        {
            // Placeholder for agenda creation logic
            return Ok(new { Message = "Agenda created successfully." });
        }
    }
}