namespace AI_Meeting_Assistant.Models
{
    public class MeetingSummaryTemplate
    {
        public string Notes { get; set; } = string.Empty;

        public List<string> Decisions { get; set; } = new();

        public List<ActionItem> ActionItems { get; set; } = new();
    }
}