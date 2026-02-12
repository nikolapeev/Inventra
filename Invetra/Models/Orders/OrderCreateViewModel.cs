using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Orders
{
    public class OrderCreateViewModel
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid CourierId { get; set; }

        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
