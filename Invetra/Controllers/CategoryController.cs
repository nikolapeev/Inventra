using Inventra.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventra.Core.ViewModels.Categories;

namespace Inventra.Controllers
{
    [Authorize(Roles = "Administrator , InventoryManager")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }   

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
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

            await _categoryService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var category =await _categoryService.GetByIdAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

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

            //var category = await _categoryService.GetByIdAsync(model.CategoryId);

            //if (category == null)
            //{
            //    return NotFound();
            //}

            //category.Name = model.Name;


            //await _context.SaveChangesAsync();
            await _categoryService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var cat= await _context.Categories.FindAsync(id);

            //if (cat == null)
            //{
            //    return NotFound();
            //}

            //_context.Categories.Remove(cat);
            //await _context.SaveChangesAsync();
            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
