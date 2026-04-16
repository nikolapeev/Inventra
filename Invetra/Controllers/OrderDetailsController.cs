using Inventra.Core.Contracts;
using Inventra.Core.Services;
using Inventra.Core.ViewModels.Categories;
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

        [HttpGet]
        public async Task<IActionResult> Create(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return NotFound();
            }

            // 2. We MUST create an instance of your exact ViewModel to prevent the NullReferenceException
            var viewModel = new OrderDetailsCreateViewModel
            {
                OrderId = orderId, // We lock the ID from the URL into the ViewModel here
                QTY = 1            // Set a safe default quantity
            };

            // 3. Populate the dropdown list so the webpage doesn't crash trying to load products
            ViewBag.ProductId = new SelectList(await _odService.GetAllProducts(), "Id", "Name");

            // 4. Pass the populated ViewModel directly into the HTML page
            //return View(viewModel);
            return PartialView("_AddProductPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailsCreateViewModel model)
        {

            ModelState.Remove("Subtotal");

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = new SelectList(await _odService.GetAllProducts(), "Id", "Name", model.ProductId);
                return View(model);
            }

            await _odService.CreateAsync(model);

            return RedirectToAction("Details", "Orders", new { id = model.OrderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid orderId, Guid productId)
        {
            await _odService.DeleteAsync(orderId, productId);

            return RedirectToAction("Details", "Orders", new { id = orderId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid orderId, Guid productId)
        {
            var od = await _odService.GetByIdAsync( orderId, productId);

            if (od== null)
            {
                return NotFound();
            }

            var model = new OrderDetailsEditViewModel
            {
                QTY = od.QTY,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDetailsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            await _odService.UpdateAsync(model);

            return RedirectToAction("Details", "Orders", new { id = model.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> EditQtyPartial(Guid orderId, Guid productId)
        {
            var od = await _odService.GetByIdAsync(orderId, productId);
            if (od == null) return NotFound();

            var model = new OrderDetailsEditViewModel
            {
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                QTY = od.QTY
            };

            return PartialView("_EditQtyPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOne(Guid orderId, Guid productId)
        {
            await _odService.DeleteOneAsync(orderId, productId);

            return RedirectToAction("Details", "Orders", new { id = orderId });

        }
    }
}
