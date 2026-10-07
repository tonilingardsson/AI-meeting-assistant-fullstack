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
            int topicCount = request.Topics.Count;
            int baseMinutesPerTopic = request.DurationMinutes / topicCount;
            int remainingMinutes = request.DurationMinutes % topicCount;

            var items = request.Topics
                .Select((topic, index) => new AgendaItem
                {
                    Order = index + 1,
                    Topic = topic,
                    DurationMinutes = baseMinutesPerTopic + (index < remainingMinutes ? 1 : 0),
                    Owner = request.Participants[index % request.Participants.Count]
                })
                .ToList();

            var response = new AgendaResponse
            {
                Message = "Tillfällig agendautkast skapat. Gemini kopplas in senare.",
                Title = request.Title,
                Purpose = request.Purpose,
                TotalDurationMinutes = request.DurationMinutes,
                Items = items,
                SummaryTemplate = new MeetingSummaryTemplate
                {
                    Notes = string.Empty,
                    Decisions = new List<string>(),
                    ActionItems = new List<ActionItem>()
                }
            };

            return Ok(response);
        }
    }
}