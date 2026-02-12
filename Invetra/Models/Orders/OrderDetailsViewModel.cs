namespace Inventra.Models.Orders
{
    /// <summary> check customer names and why it isnt id with gemini
    /// 
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CourierName { get; set; } = string.Empty;

        public string TrackingNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }

    }
}
