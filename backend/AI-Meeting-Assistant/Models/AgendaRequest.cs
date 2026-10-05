using System.ComponentModel.DataAnnotations; // Adding this for data validation attributes

namespace AI_Meeting_Assistant.Models
{
    public class AgendaRequest
    {
        // [Required]
        public string Title { get; set; } = string.Empty;
        // [Required]
        public string Purpose { get; set; } = string.Empty;
        // [Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; }
        // [Required]
        /*[MinLength(1, ErrorMessage = "At least one topic is required.")]
        public List<string> Topics { get; set; } = [];*/
        
        public List<string> Topics { get; set; } = new();
    }
}