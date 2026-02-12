using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.CourierCountries
{
    public class CourierCountriesEditViewModel
    {
        [Required]
        public Guid CourierId { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ETA { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;
    }
}
