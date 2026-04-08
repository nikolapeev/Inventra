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
    [Authorize]
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

        // GET: Couriers/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    var courier=await _context.Couriers
        //        .Where(c=>c.CourierId==id)
        //        .Select(c=>new CourierDetailsViewModel
        //        {
        //            CourierId=c.CourierId,
        //            Name=c.Name,
        //            Phone=c.Phone
        //        }).FirstOrDefaultAsync();

        //    if (courier == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(courier);
        //}

        // GET: Couriers/Create
        public IActionResult Create()
        {
            return View(new CourierCreateViewModel());
        }

        // POST: Couriers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Couriers/Edit/5
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
                Name = courier.Name
            };

            return View(model);
        }

        // POST: Couriers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        ////// GET: Couriers/Delete/5
        ////public async Task<IActionResult> Delete(Guid? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var courier = await _context.Couriers
        ////        .FirstOrDefaultAsync(m => m.CourierId == id);
        ////    if (courier == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return View(courier);
        ////}

        ////// POST: Couriers/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> DeleteConfirmed(Guid id)
        ////{
        ////    var courier = await _context.Couriers.FindAsync(id);
        ////    if (courier != null)
        ////    {
        ////        _context.Couriers.Remove(courier);
        ////    }

        ////    await _context.SaveChangesAsync();
        ////    return RedirectToAction(nameof(Index));
        ////}

        //private bool CourierExists(Guid id)
        //{
        //    return _context.Couriers.Any(e => e.CourierId == id);
        //}
    }
}
