using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventra.Data.Entities;

namespace Inventra.Data.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Courier))]
        public Guid CourierId { get; set; }
        public Courier Courier { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; } = null!;

        [Required]
        public decimal TotalPrice { get; set; } 

        //Add ETA
        public ICollection<OrderDetails> OrderDeatails { get; set; } = [];

    }
}
