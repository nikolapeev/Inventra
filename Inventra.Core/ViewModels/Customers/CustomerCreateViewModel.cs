using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Customers
{
    public class CustomerCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public string County { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string EIK { get; set; } = string.Empty;

        [Required]
        public bool ZDDS { get; set; }

        [Required]
        public string CompanyName { get; set; } = null!;

    }
}
