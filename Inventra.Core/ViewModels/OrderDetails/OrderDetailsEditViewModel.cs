using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.OrderDetails
{
    public class OrderDetailsEditViewModel
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QTY { get; set; }

        public decimal Subtotal { get; set; }
    }
}
