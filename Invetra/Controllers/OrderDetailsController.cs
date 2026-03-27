using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    [Authorize]
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
        public IActionResult Create(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return NotFound("No Order ID was passed to this page.");
            }

            // 2. We MUST create an instance of your exact ViewModel to prevent the NullReferenceException
            var viewModel = new OrderDetailsCreateViewModel
            {
                OrderId = orderId, // We lock the ID from the URL into the ViewModel here
                QTY = 1            // Set a safe default quantity
            };

            // 3. Populate the dropdown list so the webpage doesn't crash trying to load products
            ViewBag.ProductId = new SelectList(_context.Products.OrderBy(p => p.Name).ToList(), "Id", "Name");

            // 4. Pass the populated ViewModel directly into the HTML page
            return View(viewModel);
            //ViewBag.ProductId = new SelectList(_context.Products.OrderBy(p => p.Name).ToList(), "ProductId", "Name");
            //return View(new OrderDetailsCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailsCreateViewModel model)
        {
            // 1. Ignore Subtotal validation since we calculate it
            ModelState.Remove("Subtotal");

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = new SelectList(await _context.Products.OrderBy(p => p.Name).ToListAsync(), "Id", "Name", model.ProductId);
                return View(model);
            }

            var desiredProduct = await _context.Products.FindAsync(model.ProductId);
            if (desiredProduct == null) return NotFound();

            // 2. Check if this product is ALREADY in this specific order
            var existingItem = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == model.OrderId && od.ProductId == model.ProductId);

            if (existingItem != null)
            {
                // 3. If it exists, just update the existing row!
                existingItem.QTY += model.QTY;
                existingItem.Subtotal = existingItem.QTY * desiredProduct.Price;

                _context.OrderDetails.Update(existingItem);
            }
            else
            {
                // 4. If it doesn't exist yet, insert it as a brand new row
                var orderDetail = new Inventra.Data.Entities.OrderDetails
                {
                    OrderId = model.OrderId,
                    ProductId = model.ProductId,
                    QTY = model.QTY,
                    Subtotal = desiredProduct.Price * model.QTY
                };

                await _context.OrderDetails.AddAsync(orderDetail);
            }

            // 5. Save changes
            await _context.SaveChangesAsync();

            // 6. 🟢 THE MAGIC FIX: Redirect smoothly back to the order details page!
            return RedirectToAction("Details", "Orders", new { id = model.OrderId });
        }

        ////providing the ORDER id 
        //[HttpGet]
        //public async Task<IActionResult> Edit(Guid orderId)
        //{
        //    var detail = await _context.OrderDetails.FindAsync(orderId);


        //    if (detail == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new OrderDetails
        //    {
        //        OrderId = detail.OrderId,
        //        ProductId = detail.ProductId,
        //        QTY = detail.QTY,
        //        Subtotal = detail.QTY * detail.Product.Price
        //    };

        //    return View(model);
        //}

        //[HttpPost]

        //public async Task<IActionResult> Edit(OrderDetailsIndexViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model); 
        //    }

        //    var detail = await _context.OrderDetails.FindAsync(model.OrderId);

        //    if (detail == null)
        //    {
        //        return NotFound();
        //    }

        //    detail.QTY= model.QTY;
        //    detail.Subtotal= detail.QTY*detail.Product.Price;

        //    _context.OrderDetails.Update(detail);

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

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
