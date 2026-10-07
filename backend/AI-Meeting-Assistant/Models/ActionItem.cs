namespace AI_Meeting_Assistant.Models
{
    public class ActionItem
    {
        public string Description { get; set; } = string.Empty;

        public string Owner { get; set; } = string.Empty;

        public string DueDate { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}