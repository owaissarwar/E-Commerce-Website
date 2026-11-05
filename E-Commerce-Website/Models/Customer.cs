using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }= Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPassword { get; set; } = string.Empty;
        public string CustomerGender { get; set; } = string.Empty;
        public string CustomerCity { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerRole { get; set; } = "Customer";
        public string? CustomerImage { get; set; }

    }
}
