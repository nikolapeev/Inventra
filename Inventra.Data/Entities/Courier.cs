using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Courier
    {
        [Key]
        public Guid CourierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }= null!;


        public ICollection<CourierCountry> CourierCountries { get; set; } = [];
    }
}
