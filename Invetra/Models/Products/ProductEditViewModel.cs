using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Products
{
    public class ProductEditViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public string ImageURL { get; set; } = null!;

        [Required]
        public string BatchNumber { get; set; }=null!;

        [Required]
        public string WarehouseLocationId { get; set; } = null!;
    }
}
