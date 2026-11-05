using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.DTO
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product price is required.")]
        public string ProductPrice { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;
        [ValidateNever]
        public IFormFile? ProductIFormImage { get; set; }
        public string? ProductTempImage { get; set; } 

        [Required(ErrorMessage = "Please select a category.")]
        public Guid Cat_Id { get; set; }
    }
}
