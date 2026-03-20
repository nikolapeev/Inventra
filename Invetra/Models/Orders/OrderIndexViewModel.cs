namespace Inventra.Models.Orders
{
    public class OrderIndexViewModel
    {
        public Guid Id { get; set; }

        // These MUST be strings for the Index/List view to work
        public string? CustomerName { get; set; }
        public string? CourierName { get; set; }

        // Add these IDs so the Edit/Create dropdowns have a place to store data
        public Guid CustomerId { get; set; }
        public Guid CourierId { get; set; }

        public string TrackingNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
