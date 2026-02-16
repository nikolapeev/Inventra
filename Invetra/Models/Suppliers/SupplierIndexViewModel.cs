namespace Inventra.Models.Suppliers
{
    public class SupplierIndexViewModel
    {
        public Guid SupplierId { get; set; }

        public string Name { get; set; } = null!;
        public string EIK { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
