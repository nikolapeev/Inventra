using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class WarehouseLocation
    {
        [Key]
        public Guid WarehouseLocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationCode { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        //[Required]
        //public bool IsFull { get; set; }    

        public ICollection<Product> Products { get; set; }
    }
}
