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
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }    

        [Required]
        [Range(1,10000 , ErrorMessage ="Value cannot be less than 1 and more than 10000")]
        public int QTY { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}
