using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Orders
{
    public class OrderEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid CourierId { get; set; }

        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; } = null!;

        [Required]
        public decimal TotalPrice { get; set; }

    }
}
