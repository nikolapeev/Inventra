using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    [PrimaryKey(nameof(Order), nameof(Product))]
    public class OrderDeatails
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }    

        [Required]
        public int QTY { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}
