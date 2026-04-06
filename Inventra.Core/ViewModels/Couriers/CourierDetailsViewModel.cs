using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Couriers
{
    public class CourierDetailsViewModel
    {
        public Guid CourierId { get; set; }

        [MaxLength(100)]
        public string Name {  get; set; }

        [Phone]
        public string Phone { get; set; }
    }
}
