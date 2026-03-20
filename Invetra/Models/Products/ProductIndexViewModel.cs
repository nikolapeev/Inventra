namespace Inventra.Models.Products
{
    public class ProductIndexViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string BatchNumber { get; set; } = null!;

        public string WarehouseLocationId { get; set; } = null!;
    }
}

