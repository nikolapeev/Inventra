using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class GoodsReceiptLine
    {

        [Key]
        public Guid Id { get; set; }
        public GoodsReceipt GoodsReceipt { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }
    }
}
