using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    [PrimaryKey(nameof(Courier), nameof(Country))]
    public class CourierCountry
    {
        public Guid CourierId { get; set; }
        public Guid CountryId { get; set; }

        public decimal DeliveryPrice { get; set; }

        public int ETA { get; set; }

        public string Description { get; set; } = null!;    

        public Courier Courier { get; set; }
        public Country Country { get; set; }
    }
}
