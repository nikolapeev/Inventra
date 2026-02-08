using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class GoodsReceipt
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        [Required]
        public string ReceivedByUserId { get; set; } = null!;
        public InventraUser ReceivedByUser { get; set; } = null!;


        [Required]
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

        public ICollection<GoodsReceiptLine> Lines { get; set; } = new List<GoodsReceiptLine>();
    }
}
