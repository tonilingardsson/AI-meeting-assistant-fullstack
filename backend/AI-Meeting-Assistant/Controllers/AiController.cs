using Microsoft.AspNetCore.Mvc;
using AI_Meeting_Assistant.Models;
using AI_Meeting_Assistant.Services;

namespace AI_Meeting_Assistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        // Stores the service reference for the controller to use
        private readonly AgendaService _agendaService;

        //Requests the service through the constructor for dependency injection
        public AiController(AgendaService agendaService)
        {
            // Stores the instance supplied by ASP.NET Core's dependency injection system
            _agendaService = agendaService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "API is running" });
        }

        [HttpPost("agenda")]
        public IActionResult CreateAgenda([FromBody] AgendaRequest request)
        {
            // Calls the (logic or) service to create an agenda based on the request 
            AgendaResponse response = _agendaService.CreateAgenda(request);

            return Ok(response);
        }

    }
}
