using Inventra.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.CourierCountries
{
    public class CourierCountriesIndexViewModel
    {
        public string CourierName { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public decimal DeliveryPrice { get; set; }
        public int ETA { get; set; }
        public string Description { get; set; } = null!;
    }
}
