using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Orders
{
    
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CourierName { get; set; } = null!;
        public DateOnly ETA { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string AdditionalInfo { get; set; } = null!;
        public List<Data.Entities.OrderDetails> Products { get; set; } = new List<Data.Entities.OrderDetails>();
    }
}
