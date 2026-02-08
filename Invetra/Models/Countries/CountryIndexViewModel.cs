using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Countries
{
    public class CountryIndexViewModel
    {
        public Guid CountryId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
