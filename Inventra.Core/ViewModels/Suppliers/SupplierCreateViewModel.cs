using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Suppliers
{

    public class SupplierCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string EIK { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }

}
