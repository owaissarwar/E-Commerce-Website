using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; } = Guid.NewGuid();

        public Guid Prod_id { get; set; } 
        public Guid Cust_id { get; set; }

        public int Product_Quantity { get; set; }
        public int Cart_Status { get; set; }
    }
}
