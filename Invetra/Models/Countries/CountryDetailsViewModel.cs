using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Countries
{
    public class CountryDetailsViewModel
    {
        public Guid CountryId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
