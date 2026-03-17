using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.OrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly InventraDbContext _context;

        public OrderDetailsController(InventraDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == id)
                .Select(od => new OrderDetailsDetailsViewModel
                {
                    ProductName = od.Product.Name,
                    QTY = od.QTY,
                    Subtotal = od.Product.Price * od.QTY
                })
                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new OrderDetailsCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDetailsCreateViewModel model)
        {

            var desiredProduct = await _context.Products.FindAsync(model.ProductId);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var orderDetail = new OrderDetails
            {
                OrderId = model.OrderId,
                ProductId = model.ProductId,
                QTY = model.QTY,
                Subtotal = desiredProduct.Price * model.QTY

            };

            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return View(nameof(Index));
        }

        //providing the ORDER id 
        [HttpGet]
        public async Task<IActionResult> Edit(Guid orderId)
        {
            var detail = await _context.OrderDetails.FindAsync(orderId);


            if (detail == null)
            {
                return NotFound();
            }

            var model = new OrderDetails
            {
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                QTY = detail.QTY,
                Subtotal = detail.QTY * detail.Product.Price
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(OrderDetailsIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var detail = await _context.OrderDetails.FindAsync(model.OrderId);

            if (detail == null)
            {
                return NotFound();
            }

            detail.QTY= model.QTY;
            detail.Subtotal= detail.QTY*detail.Product.Price;

            _context.OrderDetails.Update(detail);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: OrderDetails/Delete?orderId={orderId}&productId={productId}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid orderId, Guid productId)
        {
            var detail = await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail == null)
            {
                return NotFound();
            }

            // Reuse the OrderDetails entity as the view model (consistent with Edit GET style)
            return View(detail);
        }

        // POST: OrderDetails/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid orderId, Guid productId)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
