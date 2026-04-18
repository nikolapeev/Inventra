using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Category
    {
        [Key]   
        public Guid CategoryId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name must be between 3 and 100 characters long.")]
        [MaxLength(100, ErrorMessage = "The name must be between 3 and 100 characters long.")]
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; }


    }
}
