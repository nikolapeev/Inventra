using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Orders
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
        [Range(1, 5000)]
        public DateOnly ETA { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string AdditionalInfo { get; set; } = null!;

    }
}
