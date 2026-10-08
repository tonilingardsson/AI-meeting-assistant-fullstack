using System.ComponentModel.DataAnnotations;

namespace AI_Meeting_Assistant.Models
{
    public class SummarizeRequest
    {
        [Required(ErrorMessage = "Meeting notes are required.")]
        [StringLength(
            20000,
            ErrorMessage = "Meeting notes cannot exceed 20,000 characters.")]
        public string Text { get; set; } = string.Empty;
    }
}