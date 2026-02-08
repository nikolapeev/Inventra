using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Couriers
{
    public class CourierDetailsViewModel
    {
        public Guid CourierId { get; set; }

        [MaxLength(100)]
        public string Name {  get; set; }
    }
}
