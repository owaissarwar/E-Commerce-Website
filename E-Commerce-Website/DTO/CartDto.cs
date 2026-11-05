using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.DTO
{
    public class CartDto
    {
        public Guid CartId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public Guid Prod_id { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid Cust_id { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Product_Quantity { get; set; }

        public int Cart_Status { get; set; }
    }
}
