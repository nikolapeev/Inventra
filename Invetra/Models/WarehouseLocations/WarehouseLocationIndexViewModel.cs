namespace Inventra.Models.WarehouseLocations
{
    public class WarehouseLocationIndexViewModel
    {
        public Guid WarehouseLocationId { get; set; }

        public string LocationCode { get; set; } = null!;
        public bool IsFull { get; set; }
    }
}
