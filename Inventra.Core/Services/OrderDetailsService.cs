using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.OrderDetails;
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
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == model.OrderId && od.ProductId == model.ProductId);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == model.OrderId);
            var product = await _context.Products.FirstOrDefaultAsync(o => o.Id == model.ProductId);

            if (existingItem != null)
            {
               
                existingItem.QTY += model.QTY;
                existingItem.Subtotal = existingItem.QTY * desiredProduct.Price;
                product.StockQuantity -= model.QTY;

                order.TotalPrice += existingItem.Subtotal;

                _context.OrderDetails.Update(existingItem);
                _context.Products.Update(product);
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

                if (product.StockQuantity < model.QTY)
                {
                    return;
                }
                product.StockQuantity -= model.QTY;
                order.TotalPrice += orderDetail.Subtotal;
                _context.Orders.Update(order);
                _context.Products.Update(product);

                await _context.OrderDetails.AddAsync(orderDetail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid orderId, Guid productId)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            var order = await _context.Orders.FindAsync(orderId);
            var product = await _context.Products.FindAsync(productId);

            if (detail == null)
            {
                return;
            }

            order.TotalPrice -= detail.Subtotal;
            product.StockQuantity += detail.QTY;

            _context.Orders.Update(order);
            _context.Products.Update(product);
            _context.OrderDetails.Remove(detail);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOneAsync(Guid orderId, Guid productId)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            var order = await _context.Orders.FindAsync(orderId);
            var product = await _context.Products.FindAsync(productId);

            if (detail.QTY <= 1)
            {
                await DeleteAsync(orderId, productId);
            }

            if (detail == null)
            {
                return;
            }

            detail.QTY -= 1;

            product.StockQuantity += 1;

            detail.Subtotal = detail.QTY * product.Price;

            order.TotalPrice -= product.Price;



            _context.OrderDetails.Update(detail);
            _context.Orders.Update(order);
            _context.Products.Update(product);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.OrderBy(p => p.TrackingNumber).ToListAsync();
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Where(p => p.StockQuantity > 0).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<OrderDetails?> GetByIdAsync(Guid orderId, Guid productId)
        {
            return await _context.OrderDetails
        .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId); ;
        }

        public async Task UpdateAsync(OrderDetailsEditViewModel model)
        {

            
            var od = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == model.OrderId && x.ProductId == model.ProductId);

            if (od is null)
            {
                return;
            }

            int quantityDifference = model.QTY - od.QTY;

            if (quantityDifference == 0)
            {
                return; 
            }

            if (quantityDifference > 0 && quantityDifference > od.Product.StockQuantity)
            {
                return;
            }

           
            od.Product.StockQuantity -= quantityDifference;

            od.QTY = model.QTY;
            od.Subtotal = od.QTY * od.Product.Price;

            decimal priceDifference = quantityDifference * od.Product.Price;
            od.Order.TotalPrice += priceDifference;

            await _context.SaveChangesAsync();
        }
    }
}
