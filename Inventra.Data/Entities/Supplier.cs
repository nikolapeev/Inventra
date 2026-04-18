using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Supplier
    {
        [Key]
        public Guid SupplierId { get; set; }

        [Required]
        [StringLength(50 , MinimumLength =2)]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters , numbers and hyphens.")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(10,ErrorMessage ="The EIK must be between 9 and 10 characters long.")]
        [MinLength(9, ErrorMessage = "The EIK must be between 9 and 10 characters long.")]
        [RegularExpression(@"^[0-9]+$")]
        public string EIK { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;


        public ICollection<Product> Products { get; set; }
    }
}
