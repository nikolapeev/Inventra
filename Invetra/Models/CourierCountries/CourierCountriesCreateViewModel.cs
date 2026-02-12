using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.CourierCountries
{
    public class CourierCountriesCreateViewModel
    {
        [Required]
        public Guid CourierId { get; set; }
        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }
        [Required]
        public int ETA { get; set; }
        [Required]
        public string Description { get; set; } = null!;
    }
}
