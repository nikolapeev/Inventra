using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.OrderDetails;
using Inventra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly InventraDbContext _context;
        public OrderDetailsService(InventraDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(OrderDetailsCreateViewModel model)
        {
            var desiredProduct = await _context.Products.FindAsync(model.ProductId);
            if (desiredProduct == null) 
            {
                return; 
            }

            // Check if this product is already in this specific order
            var existingItem = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == model.OrderId && od.ProductId == model.ProductId);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (existingItem != null)
            {
                existingItem.QTY += model.QTY;
                existingItem.Subtotal = existingItem.QTY * desiredProduct.Price;

                order.TotalPrice += existingItem.Subtotal;

                _context.OrderDetails.Update(existingItem);
                _context.Orders.Update(order);
            }
            else
            {
                var orderDetail = new Inventra.Data.Entities.OrderDetails
                {
                    OrderId = model.OrderId,
                    ProductId = model.ProductId,
                    QTY = model.QTY,
                    Subtotal = desiredProduct.Price * model.QTY
                };

                order.TotalPrice += orderDetail.Subtotal;
                _context.Orders.Update(order);

                await _context.OrderDetails.AddAsync(orderDetail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid orderId, Guid productId)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail == null)
            {
                return;
            }

            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();
        }
    }
}
