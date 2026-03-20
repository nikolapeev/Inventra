using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Orders
{
    public class OrderCreateViewModel
    {
        public Guid Id { get; set; }

        // 1. The "Working" IDs (For the Dropdowns/Database)
        public Guid CustomerId { get; set; }
        public Guid CourierId { get; set; }

        // 2. The "Display" Names (For the List/Tables)
        // We make these nullable (?) or 'null!' so they don't break the form
        public string? CustomerName { get; set; }
        public string? CourierName { get; set; }

        public string TrackingNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string? AdditionalInfo { get; set; }

    }
}
