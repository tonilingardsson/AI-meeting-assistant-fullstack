using Microsoft.AspNetCore.Mvc;
using AI_Meeting_Assistant.Models;

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
            return Ok(
                // Temporary JSON response because "new" creates an anonymous type,
                // and "Ok" serializes it to JSON with HTTP 200 status code.
                // IAction result is useful when an endpoint may return different types
                // of responses, such as ok, badRequest, or an error response.
                new
                {  
                Message = "Tillfälligt agendautkast skapat. Gemini kopplas in senare.",
                Title = request.Title,
                Purpose = request.Purpose,
                DurationMinutes = request.DurationMinutes,
                Topics = request.Topics
            });
        }
    }
}