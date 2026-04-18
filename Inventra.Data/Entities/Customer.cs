using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Field can only contain letters and hyphens.")]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Field can only contain letters and hyphens.")]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Field can only contain letters and hyphens.")]

        public string County { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Field can only contain letters and hyphens.")]

        public string City { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters , numbers and hyphens.")]
        [MinLength(3,ErrorMessage = "The Address must be between 3 and 50 characters")]
        [MaxLength(50, ErrorMessage = "The Address code must be between 3 and 50 characters")]
        public string Address { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters , numbers and hyphens.")]
        [MinLength(3,ErrorMessage ="The postal code must be between 3 and 20 characters")]
        [MaxLength(20, ErrorMessage = "The postal code must be between 3 and 20 characters")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Field can only contain letters and numbers .")]
        [MinLength(9,ErrorMessage ="EIK must be between 9 and 10 characters")]
        [MaxLength(10,ErrorMessage ="EIK must be between 9 and 10 characters")]
        public string EIK { get; set; } = null!;   

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters, numbers and hyphens.")]
        [MinLength(3, ErrorMessage = "The company name must be between 3 and 50 characters")]
        [MaxLength(50, ErrorMessage = "The company name must be between 3 and 50 characters")]
        public string CompanyName { get; set; } = null!;

        [Required]
        public bool ZDDS { get; set; }

    }
}
