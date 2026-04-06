using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Couriers
{
    public class CourierIndexViewModel
    {
        public Guid CourierId {  get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null;

        [Phone]
        public string Phone { get; set; } = null!;
    }
}
