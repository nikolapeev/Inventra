using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventra.Data.Entities;
using Inventra.Data.Enums;

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
        [MaxLength(100,ErrorMessage ="The tracking number can be a maximum of 100 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Field can only contain letters , numbers and hyphens.")]
        public string TrackingNumber { get; set; } = null!;

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [MinLength(10)]
        public string AdditionalInfo { get; set; } = null!;

        [Required]
        [Range(1,5000)]
        public DateOnly ETA { get; set;  }

        [Required]
        public Statuses Status { get; set; } 

        public ICollection<OrderDetails> OrderDetails { get; set; } = [];

    }
}
