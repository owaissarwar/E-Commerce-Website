using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Feedback
    {
        [Key]
        public Guid FeedbackId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string UserMessage { get; set; } = string.Empty;
    }
}
