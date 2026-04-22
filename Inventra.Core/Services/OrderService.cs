using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Orders;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly InventraDbContext _context;
        public OrderService(InventraDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(OrderCreateViewModel model)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = model.CustomerId,
                CourierId = model.CourierId,
                TrackingNumber = model.TrackingNumber,
                AdditionalInfo = model.AdditionalInfo,
                Status = model.Status,
                ETA = model.ETA
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderIndexViewModel>> GetAllOrders(string? searchTerm = null)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o => o.Customer.CompanyName.Contains(searchTerm) || o.TrackingNumber.Contains(searchTerm));
            }

            return await query
            .Select(o => new OrderIndexViewModel
            {
             Id = o.Id,
             CustomerName = o.Customer.FullName,
             CourierName = o.Courier.Name,
             TrackingNumber = o.TrackingNumber,
             TotalPrice = o.TotalPrice,
             Status = o.Status,
             ETA = o.ETA
            }).ToListAsync();
        }

        public Task<List<Courier>> GetCourierList()
        {
            return _context.Couriers.OrderBy(x => x.Name).ToListAsync();
        }

        public Task<List<Customer>> GetCustomerList()
        {
            return _context.Customers.OrderBy(x => x.FullName).ToListAsync();
        }

        public async Task<OrderDetailsViewModel?> GetDetailsById(Guid id)
        {
            var order = await _context.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.Courier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return null;
            }


            var orderItems = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == id)
                .ToListAsync();


            /*var viewModel =*/
            return new Inventra.Core.ViewModels.Orders.OrderDetailsViewModel
            {
                Id = order.Id,
                CustomerName = order.Customer?.FullName ?? "NO customer found",
                CompanyName = order.Customer?.CompanyName,
                Phone = order.Customer.PhoneNumber,
                Email = order.Customer.Email,
                CourierName = order.Courier.Name,
                TrackingNumber = order.TrackingNumber,
                AdditionalInfo = order.AdditionalInfo,
                TotalPrice = order.TotalPrice,
                Products = orderItems,
                ETA = order.ETA,
            };

        }

        public async Task UpdateAsync(OrderIndexViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.CustomerId = model.CustomerId;
            order.CourierId = model.CourierId;
            order.TrackingNumber = model.TrackingNumber;
            order.AdditionalInfo = model.AdditionalInfo;
            order.ETA = model.ETA;
            order.Status = model.Status;
            //order.TotalPrice = model.TotalPrice;

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id)) return;
                else throw;
            }
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public bool OrderExists(Guid id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }

        public async Task<decimal> SumOrderTotal(Guid id)
        {
            decimal total = 0;
            var details = await _context.OrderDetails.Where(od => od.OrderId == id).ToListAsync();
            foreach (var detail in details)
            {
                total += detail.Subtotal;
            }
            return total;
        }
    }
}
