using System;

namespace Inventra.Core.ViewModels.Suppliers
{

    public class SupplierDetailsViewModel
    {
        public Guid SupplierId { get; set; }

        public string Name { get; set; } = null!;
        public string EIK { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

    }

}
