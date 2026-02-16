using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.WarehouseLocations
{
    public class WarehouseLocationEditViewModel
    {
        [Required]
        public Guid WarehouseLocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsFull { get; set; }
    }
}
