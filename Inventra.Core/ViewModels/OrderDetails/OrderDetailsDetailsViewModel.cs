namespace Inventra.Core.ViewModels.OrderDetails
{
    public class OrderDetailsDetailsViewModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int QTY { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
