using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Faq
    {
        [Key]
        public Guid FaqId { get; set; } = Guid.NewGuid();

        public string FaqQuestion { get; set; } = string.Empty;

        public string FaqAnswer { get; set; } = string.Empty;
    }
}