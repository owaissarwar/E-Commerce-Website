using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; } = string.Empty;
        public string ProductPrice { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string? ProductImage { get; set; } 
        public Guid Cat_Id { get; set; }
        public Category Category { get; set; } = null!;
    }
}
