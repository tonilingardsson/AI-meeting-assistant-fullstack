namespace AI_Meeting_Assistant.Models
{
    // This is the object that the frontend will eventually receive
    public class AgendaResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int TotalDurationMinutes { get; set; }
        public List<AgendaItem> Items { get; set; } = new();
    }
}
