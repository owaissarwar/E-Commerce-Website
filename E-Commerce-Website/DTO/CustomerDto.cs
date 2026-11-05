using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.DTO
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        [Required]
        public string CustomerHashPassword { get; set; } = string.Empty;
        [Required]
        public string CustomerGender { get; set; } = string.Empty;
        [Required]
        public string CustomerCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 500 characters long.")]
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerRole { get; set; } = "Customer";
        public IFormFile? CustomerIFormImage { get; set; }
        public string CustTempImage { get; set; } = string.Empty;
    }
}
