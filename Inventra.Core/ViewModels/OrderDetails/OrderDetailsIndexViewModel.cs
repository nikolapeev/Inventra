using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.OrderDetails
{
    public class OrderDetailsIndexViewModel
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int QTY { get; set; }

        public decimal Subtotal { get; set; }
    }
}
