using System;
using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Suppliers
{

    public class SupplierEditViewModel
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string EIK { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

}
