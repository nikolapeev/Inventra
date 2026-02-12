namespace Inventra.Models.Orders
{
    public class OrderIndexViewModel
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CourierName { get; set; } = null!;

        public string TrackingNumber { get; set; } = null!; 
        public decimal TotalPrice { get; set; }
    }
}
