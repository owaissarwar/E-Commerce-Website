using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.DTO
{
    public class FeedbackDto
    {
        public Guid FeedbackId { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User message is required.")]
        public string UserMessage { get; set; } = string.Empty;
    }
}
