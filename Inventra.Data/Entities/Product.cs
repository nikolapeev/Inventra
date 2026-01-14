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
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

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
        public string BatchNumber { get; set; } = null!;    

        //[Required]
        //[ForeignKey(nameof(Supplier))]  
        //public int SupplierId { get; set; }
        //public Supplier Supplier { get; set; } = null!;

        //[Required]
        //public string AddedBy { get; set; } = null!;    

        [Required]
        [ForeignKey(nameof(WarehouseLocation))] 
        public Guid WarehouseLocationId { get; set; }
        public WarehouseLocation WarehouseLocation { get; set; } = null!;


        public ICollection<OrderDetails> OrderDeatails { get; set; } = [];

    }
}
