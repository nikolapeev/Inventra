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
                if (product.StockQuantity < model.QTY)
                {
                    return;
                }
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
                //.Include(o=>o.Order)
                //.Include(p=>p.Product)
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

            //detail.Order.TotalPrice -= detail.QTY * detail.Product.Price;


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

        //public async Task UpdateAsync(OrderDetailsEditViewModel model)
        //{
        //    var od = await _context.OrderDetails
        //    .FirstOrDefaultAsync(x => x.OrderId == model.OrderId && x.ProductId == model.ProductId);

        //    var order = await _context.Orders.FindAsync(model.OrderId);
        //    var product = await _context.Products.FindAsync(model.ProductId);

        //    if (od != null)
        //    {
        //        //od.QTY = model.QTY;

        //        if (od.QTY > model.QTY)
        //        {
        //            int diff = od.QTY - model.QTY;
        //            order.TotalPrice -= diff * product.Price;
        //            product.StockQuantity += diff;
        //            od.Subtotal -= model.Subtotal;
        //        }
        //        else if (od.QTY < model.QTY)
        //        {
        //            int diff = model.QTY - od.QTY;
        //            product.StockQuantity -= diff;
        //            od.Subtotal += model.Subtotal;
        //            order.TotalPrice += diff * product.Price;
        //        }

        //        //od.Order.TotalPrice=model.QTY*od.Product.Price;
        //        _context.Orders.Update(order);
        //        _context.Products.Update(product);
        //        _context.OrderDetails.Update(od);

        //        await _context.SaveChangesAsync();
        //    }


        //}

        public async Task UpdateAsync(OrderDetailsEditViewModel model)
        {

            //var existingItem = await _context.OrderDetails
            //    .FirstOrDefaultAsync(od => od.OrderId == model.OrderId && od.ProductId == model.ProductId);

            //var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == model.OrderId);
            //var product = await _context.Products.FirstOrDefaultAsync(o => o.Id == model.ProductId);

            //if (existingItem != null)
            //{
            //    if (product.StockQuantity < model.QTY)
            //    {
            //        return;
            //    }
            //    existingItem.QTY += model.QTY;
            //    existingItem.Subtotal = existingItem.QTY * product.Price;
            //    product.StockQuantity -= model.QTY;

            //    order.TotalPrice += existingItem.Subtotal;

            //    _context.OrderDetails.Update(existingItem);
            //    _context.Products.Update(product);
            //    _context.Orders.Update(order);
            //}

            //await _context.SaveChangesAsync();
            // 1. Fetch the OrderDetail along with the Order and Product in a single query
            // using Include() to minimize database round-trips.
            var od = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == model.OrderId && x.ProductId == model.ProductId);

            // If the record doesn't exist, exit early.
            if (od is null)
            {
                return;
            }

            // Calculate the difference. 
            // Positive = user wants more. Negative = user wants less.
            int quantityDifference = model.QTY - od.QTY;

            if (quantityDifference == 0)
            {
                return; // No changes made, nothing to update.
            }

            // 2. Stock Validation Constraint
            if (quantityDifference > 0 && quantityDifference > od.Product.StockQuantity)
            {
                // Throwing an exception prevents the transaction from continuing. 
                // You should catch this in your Controller/UI to show a friendly error message.
                //throw new InvalidOperationException($"Insufficient stock. Only {od.Product.StockQuantity} items available.");
                return;
            }

            // 3. Update Product Stock
            // If difference is positive (buying 2 more), stock goes down by 2.
            // If difference is negative (returning 2), subtracting a negative adds to the stock.
            od.Product.StockQuantity -= quantityDifference;

            // 4. Update the OrderDetail
            od.QTY = model.QTY;
            od.Subtotal = od.QTY * od.Product.Price;

            // 5. Update the Order Total
            decimal priceDifference = quantityDifference * od.Product.Price;
            od.Order.TotalPrice += priceDifference;

            // 6. Save changes
            // Because EF Core tracks these entities, it knows exactly which columns changed.
            await _context.SaveChangesAsync();
        }
    }
}
