using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.OrderDetails;
using Inventra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly IOrderDetailsService _odService;

        public OrderDetailsController(IOrderDetailsService odService)
        {
            _odService = odService;
        }

        //public async Task<IActionResult> Details(Guid id)
        //{
        //    var orderDetails = await _context.OrderDetails
        //        .Where(od => od.OrderId == id)
        //        .Select(od => new OrderDetailsDetailsViewModel
        //        {
        //            ProductName = od.Product.Name,
        //            QTY = od.QTY,
        //            Subtotal = od.Product.Price * od.QTY
        //        })
        //        .ToListAsync();

        //    if (orderDetails == null || !orderDetails.Any())
        //    {
        //        return NotFound();
        //    }

        //    return View(orderDetails);
        //}

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

            ModelState.Remove("Subtotal");

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = new SelectList(await _context.Products.OrderBy(p => p.Name).ToListAsync(), "Id", "Name", model.ProductId);
                return View(model);
            }

            //var desiredProduct = await _context.Products.FindAsync(model.ProductId);
            //if (desiredProduct == null) return NotFound();

            //// Check if this product is already in this specific order
            //var existingItem = await _context.OrderDetails
            //    .FirstOrDefaultAsync(od => od.OrderId == model.OrderId && od.ProductId == model.ProductId);

            //var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == model.OrderId);

            //if (existingItem != null)
            //{
            //    existingItem.QTY += model.QTY;
            //    existingItem.Subtotal = existingItem.QTY * desiredProduct.Price;

            //    order.TotalPrice += existingItem.Subtotal;

            //    _context.OrderDetails.Update(existingItem);
            //    _context.Orders.Update(order);
            //}
            //else
            //{
            //    var orderDetail = new Inventra.Data.Entities.OrderDetails
            //    {
            //        OrderId = model.OrderId,
            //        ProductId = model.ProductId,
            //        QTY = model.QTY,
            //        Subtotal = desiredProduct.Price * model.QTY
            //    };

            //    order.TotalPrice += orderDetail.Subtotal;
            //    _context.Orders.Update(order);

            //    await _context.OrderDetails.AddAsync(orderDetail);
            //}

            //await _context.SaveChangesAsync();

            await _odService.CreateAsync(model);

            return RedirectToAction("Details", "Orders", new { id = model.OrderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid orderId, Guid productId)
        {
            await _odService.DeleteAsync(orderId, productId);

            return RedirectToAction(nameof(Index));
        }
    }
}
