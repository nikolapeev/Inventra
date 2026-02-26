using System.Globalization;

namespace Inventra.Models.WarehouseLocations
{
    public class WarehouseLocationIndexViewModel
    {
        public Guid WarehouseLocationId { get; set; }

        public string Description { get; set; }=null!;
        public string LocationCode { get; set; } = null!;
        public bool IsFull { get; set; }
    }
}
