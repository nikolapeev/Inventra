using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Countries
{
    public class CountryCreateViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
