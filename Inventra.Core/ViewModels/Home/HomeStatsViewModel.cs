using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.ViewModels.Home
{
    public class HomeStatsViewModel
    {
        public List<Inventra.Core.ViewModels.Messages.MessageIndexViewModel> CrucialMessages { get; set; } = new();
        public List<Inventra.Core.ViewModels.Messages.MessageIndexViewModel> InfoMessages { get; set; } = new();
        public List<TopCustomerViewModel> TopCustomers { get; set; } = new();
        public List<ProductStatViewModel> MostPurchased { get; set; } = new();
        public int LowStockProductsCount { get; set; }

        public int TotalOrders { get; set; }
        public int ProcessedOrdersCount { get; set; }
        public int InProgressOrdersCount { get; set; }
        public int ShippedOrdersCount { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
