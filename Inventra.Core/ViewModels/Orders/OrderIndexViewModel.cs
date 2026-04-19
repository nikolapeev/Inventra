using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Orders
{
    public class OrderIndexViewModel
    {
        public Guid Id { get; set; }
        public DateOnly ETA { get; set; }

        public string? CustomerName { get; set; }
        public string? CourierName { get; set; }

        public Guid CustomerId { get; set; }
        public Guid CourierId { get; set; }

        public string TrackingNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
