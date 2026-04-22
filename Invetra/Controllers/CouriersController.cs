using Inventra.Core.Contracts;
using Inventra.Core.Services;
using Inventra.Core.ViewModels.Couriers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    [Authorize(Roles ="Administrator,OrderManager")]
    public class CouriersController : Controller
    {
        private readonly ICourierService _courierService;

        public CouriersController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        // GET: Couriers
        public async Task<IActionResult> Index()
        {
            var couriers = await _courierService.GetAllAsync();
            return View(couriers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CourierCreateViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            await _courierService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var courier = await _courierService.GetByIdAsync(id);

            if (courier == null)
            {
                return NotFound();
            }

            var model = new CourierIndexViewModel
            {
                CourierId = courier.CourierId,
                Name = courier.Name,
                Phone=courier.Phone
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourierIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _courierService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _courierService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
