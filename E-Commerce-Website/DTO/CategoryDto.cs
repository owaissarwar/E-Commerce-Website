using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace E_Commerce_Website.DTO
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; } = string.Empty;

        public string? TempImage { get; set; }

        public IFormFile? CategoryImageFile { get; set; }
    }
}