namespace Inventra.Models.Orders
{
    /// <summary> check customer names and why it isnt id with gemini
    /// 
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CourierName { get; set; } = null!;

        public string TrackingNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string AdditionalInfo { get; set; } = null!;
        public List<Inventra.Data.Entities.OrderDetails> Products { get; set; } = new List<Inventra.Data.Entities.OrderDetails>();
    }
}
