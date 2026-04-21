using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventra.Controllers
{
    [Authorize(Roles ="Administrator, InventoryManager")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var products = await _productService.GetAllAsync(searchTerm);
            ViewBag.CurrentSearch = searchTerm;
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productService.GetDetailsByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.SupplierId = new SelectList(await _productService.ListSupplier(), "SupplierId", "Name");
            ViewBag.CategoryId = new SelectList(await _productService.ListCategory(), "CategoryId", "Name");

            return View(new ProductCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SupplierId = new SelectList(await _productService.ListSupplier(), "SupplierId", "Name", model.SupplierId);
                ViewBag.CategoryId = new SelectList(await _productService.ListCategory(), "CategoryId", "Name", model.CategoryId);

                return View(model);
            }

            await _productService.CreateAsync(model , User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.SupplierId = new SelectList(await _productService.ListSupplier(), "SupplierId", "Name");
            ViewBag.CategoryId = new SelectList(await _productService.ListCategory(), "CategoryId", "Name");

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageURL = product.ImageURL,
                BatchNumber = product.BatchNumber,
                WarehouseLocationId = product.WarehouseLocationId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await _productService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
