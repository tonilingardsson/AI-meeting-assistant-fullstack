using System.ComponentModel.DataAnnotations;

namespace AI_Meeting_Assistant.Models
{
    public class InvitationRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose is required.")]
        [StringLength(500, ErrorMessage = "Purpose cannot exceed 500 characters.")]
        public string Purpose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Meeting time is required.")]
        [StringLength(50, ErrorMessage = "Meeting time cannot exceed 50 characters.")]
        public string When { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; } = string.Empty;
    }
}