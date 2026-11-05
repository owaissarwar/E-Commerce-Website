using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.DTO
{
    public class FaqDto
    {
        public Guid FaqId { get; set; }

        [Required(ErrorMessage = "Question is required.")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Answer is required.")]
        public string Answer { get; set; } = string.Empty;
    }
}
