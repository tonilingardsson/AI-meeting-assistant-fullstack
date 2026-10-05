using System.ComponentModel.DataAnnotations;

namespace AI_Meeting_Assistant.Models
{
    public class AgendaRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose is required.")]
        [StringLength(500, ErrorMessage = "Purpose cannot be longer than 500 characters.")]
        public string Purpose { get; set; } = string.Empty;

        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes.")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "At least one topic is required.")]
        [MinLength(1, ErrorMessage = "At least one topic is required.")]
        public List<string> Topics { get; set; } = new();

        [Required(ErrorMessage = "At least one participant is required.")]
        [MinLength(1, ErrorMessage = "At least one participant is required.")]
        public List<string> Participants { get; set; } = new();
    }
}