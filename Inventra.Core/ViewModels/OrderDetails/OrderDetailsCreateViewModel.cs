using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.OrderDetails
{
    public class OrderDetailsCreateViewModel
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QTY { get; set; }

        //is calculated
        public decimal Subtotal { get; set; }

    }
}
