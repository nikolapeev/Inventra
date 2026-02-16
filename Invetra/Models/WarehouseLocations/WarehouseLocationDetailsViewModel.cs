namespace Inventra.Models.WarehouseLocations
{
    public class WarehouseLocationDetailsViewModel
    {
        public Guid WarehouseLocationId { get; set; }

        public string LocationCode { get; set; } = null!;
        public string Description { get; set; } = null!;

        public bool IsFull { get; set; }
    }
}
