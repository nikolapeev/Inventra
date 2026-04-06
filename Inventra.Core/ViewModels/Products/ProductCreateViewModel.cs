using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Products
{
    public class ProductCreateViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please select a category")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a supplier")]
        public Guid SupplierId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Stock cannot be less than one")]
        public int StockQuantity { get; set; }

        [Required]
        public string ImageURL { get; set; } = null!;

        [Required]
        public string BatchNumber { get; set; } = null!;

        [Required]
        public string WarehouseLocationId { get; set; } = null!;
    }
}
