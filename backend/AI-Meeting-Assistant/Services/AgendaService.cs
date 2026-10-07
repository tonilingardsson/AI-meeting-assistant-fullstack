using AI_Meeting_Assistant.Models;

namespace AI_Meeting_Assistant.Services
{
    public class AgendaService
    {
        public AgendaResponse CreateAgenda(AgendaRequest request)
        {
            int topicCount = request.Topics.Count;
            int baseMinutesPerTopic = request.DurationMinutes / topicCount;
            int remainingMinutes = request.DurationMinutes % topicCount;

            var items = request.Topics
                .Select((topic, index) => new AgendaItem
                {
                Order = index + 1,
                    Topic = topic,
                    DurationMinutes = baseMinutesPerTopic
                    + (index < remainingMinutes ? 1 : 0),
                Owner = request.Participants[
                    index % request.Participants.Count]
                    })
            .ToList();

            return new AgendaResponse
            {
                Message = "Tillfällig agendautkast skapat. Gemini kopplas in senare.",
                Title = request.Title,
                Purpose = request.Purpose,
                Items = items,
                SummaryTemplate = new MeetingSummaryTemplate
                {
                    Notes = string.Empty,
                    Decisions = new List<string>(),
                    ActionItems = new List<ActionItem>()
                }
            };
        }
    }
}
