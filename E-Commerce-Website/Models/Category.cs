using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; } = Guid.NewGuid();

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        // Image property add ki gayi hai
        public string? CategoryImage { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}