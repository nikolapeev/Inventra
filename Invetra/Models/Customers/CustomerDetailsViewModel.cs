namespace Inventra.Models.Customers
{
    public class CustomerDetailsViewModel
    {
        public Guid CustomerId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public string EIK { get; set; } = string.Empty;
        public string ZDDS { get; set; } = string.Empty;
    }
}
