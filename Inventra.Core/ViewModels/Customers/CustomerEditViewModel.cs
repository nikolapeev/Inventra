using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Customers
{
    public class CustomerEditViewModel
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string County { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public string EIK { get; set; } = null!;

        [Required]
        public string ZDDS { get; set; } = null!;
    }
}
