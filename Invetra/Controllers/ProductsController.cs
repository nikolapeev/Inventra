using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Products;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            
            var products =await _productService.GetAllAsync();

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productService.GetDetailsByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.SupplierId = new SelectList(await _productService.ListSupplier(), "SupplierId", "Name");
            ViewBag.CategoryId = new SelectList(await _productService.ListCategory(), "CategoryId", "Name");

            return View(new ProductCreateViewModel());
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            //var product = new Product
            //{
            //    Id = Guid.NewGuid(),
            //    Name = model.Name,
            //    CategoryId = model.CategoryId,
            //    SupplierId = model.SupplierId, 
            //    Description = model.Description,
            //    Price = model.Price,
            //    StockQuantity = model.StockQuantity,
            //    ImageURL = model.ImageURL,
            //    BatchNumber = model.BatchNumber,
            //    WarehouseLocationId = model.WarehouseLocationId,
            //    AddedBy = User.Identity?.Name ?? "System" // Added this as your Entity marks it [Required]
            //};

            //await _context.Products.AddAsync(product);
            //await _context.SaveChangesAsync();

            await _productService.CreateAsync(model , User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
