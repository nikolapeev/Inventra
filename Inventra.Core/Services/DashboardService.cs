using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Home;
using Inventra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly InventraDbContext _context;

        public DashboardService(InventraDbContext context)
        {
            _context = context;
        }

        public async Task<HomeStatsViewModel> GetHomeStatsAsync()
        {
            var topCustomers = await _context.Orders
                .GroupBy(o => o.Customer.FullName)
                .Select(g => new TopCustomerViewModel
                {
                    Name = g.Key,
                    TotalSpent = g.Sum(o => o.TotalPrice)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();

            var mostPurchased = await _context.OrderDetails
                .GroupBy(od => od.Product.Name)
                .Select(g => new ProductStatViewModel
                {
                    ProductName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(p => p.OrderCount)
                .Take(5)
                .ToListAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.TotalPrice);
            
            var crucialMessages = await _context.Messages
                .Where(m => m.Type == Inventra.Data.Enums.MessageType.Crucial) 
                .OrderByDescending(m => m.CreatedAt) 
                .Take(3)
                .Select(m => new Inventra.Core.ViewModels.Messages.MessageIndexViewModel
                {
                    Id = m.Id,
                    CreatedAt = m.CreatedAt,
                    CreatedBy = m.CreatedBy,
                    Content = m.Content,
                })
                .ToListAsync();

            
            var infoMessages = await _context.Messages
                .Where(m => m.Type == Inventra.Data.Enums.MessageType.Info)
                .OrderByDescending(m => m.Id)
                .Take(3)
                .Select(m => new Inventra.Core.ViewModels.Messages.MessageIndexViewModel
                {
                    Id = m.Id,
                    CreatedAt = m.CreatedAt,
                    CreatedBy = m.CreatedBy,
                    Content = m.Content,
                })
                .ToListAsync();

            var lowStockProductsCount = await _context.Products
                .Where(p => p.StockQuantity < 5)
                .CountAsync();

            var totalOrders = await _context.Orders.CountAsync();

            var processedCount = await _context.Orders
                .CountAsync(o => o.Status == Inventra.Data.Enums.Statuses.Processed);

            var inProgressCount = await _context.Orders
                .CountAsync(o => o.Status == Inventra.Data.Enums.Statuses.InProgress);

            var shippedCount = await _context.Orders
                .CountAsync(o => o.Status == Inventra.Data.Enums.Statuses.Shipped);


            return new HomeStatsViewModel
            {
                TopCustomers = topCustomers,
                MostPurchased = mostPurchased,
                TotalRevenue = totalRevenue,
                CrucialMessages = crucialMessages,
                InfoMessages = infoMessages,
                LowStockProductsCount= lowStockProductsCount,
                TotalOrders = totalOrders,
                ProcessedOrdersCount = processedCount,
                InProgressOrdersCount = inProgressCount,
                ShippedOrdersCount = shippedCount
            };
        }
    }
}
