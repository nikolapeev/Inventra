namespace Inventra.Core.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageURL { get; set; } = null!;
        public string SupplierName {  get; set; } = null!;
        public string BatchNumber { get; set; } = null!;
        public string WarehouseLocationId { get; set; } = null!;
        public string AddedBy { get; set; } = null!;
    }
}
