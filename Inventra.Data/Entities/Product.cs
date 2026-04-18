using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="The name must be between 3 and 500 characters long")]
        [MaxLength(500, ErrorMessage = "The name must be between 3 and 500 characters long")]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        [StringLength(5000,MinimumLength =20, ErrorMessage ="Description must be between 20 and 5000 characters long")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01,100000,ErrorMessage = "Value must be between 0.01 and 100000")]
        public decimal Price { get; set; } 

        [Required]
        [Range(1, 100000, ErrorMessage = "Value must be between 1 and 100000")]
        public int StockQuantity { get; set; }

        [Required]
        public string ImageURL { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Supplier))]
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters , numbers and hyphens.")]
        public string BatchNumber { get; set; } = null!;

        [Required]
        public string AddedBy { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[A-Z0-9\-]+$",ErrorMessage ="Location ID can contain only capital letters , numbers and a hyphen/minus")]
        public string WarehouseLocationId { get; set; } = null!;


        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    }
}


