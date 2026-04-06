namespace Inventra.Core.ViewModels.Customers
{
    public class CustomerIndexViewModel
    {
        public Guid CustomerId { get; set; }

        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;

        public string EIK { get; set; } = null!;
        public string ZDDS { get; set; } = null!;
    }
}
