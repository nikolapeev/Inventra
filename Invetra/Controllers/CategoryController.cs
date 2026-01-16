using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    public class CategoryController : Controller
    {
        private readonly InventraDbContext _context;

        public CategoryController(InventraDbContext context)
        {
            _context = context;
        }   

        public async Task<IActionResult> Index()
        {
            var categories= await _context.Categories
                .Select(c=>new CategoryIndexViewModel
                {
                    CategoryId=c.CategoryId,
                    Name=c.Name
                })
                .ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        public async Task <IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = model.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDetailsViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryIndexViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await _context.Categories.FindAsync(model.CategoryId);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
