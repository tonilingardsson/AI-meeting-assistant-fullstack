namespace AI_Meeting_Assistant.Models
{
    public class AgendaItem
    {
        public int Order { get; set; }
        public string Topic { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string Owner { get; set; } = string.Empty;
    }
}
