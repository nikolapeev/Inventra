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
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.SupplierId = new SelectList(await _context.Suppliers.OrderBy(s => s.Name).ToListAsync(), "SupplierId", "Name");
            ViewBag.CategoryId = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name");

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
                ViewBag.SupplierId = new SelectList(await _context.Suppliers.OrderBy(s => s.Name).ToListAsync(), "SupplierId", "Name", model.SupplierId);
                ViewBag.CategoryId = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", model.CategoryId);

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

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var product = await _context.Products.FindAsync(id);

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

           var product = await _context.Products.FindAsync(model.Id);   

            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;
            product.ImageURL = model.ImageURL;
            product.BatchNumber = model.BatchNumber;
            product.WarehouseLocationId = model.WarehouseLocationId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.WarehouseLocationId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
