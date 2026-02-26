using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.WarehouseLocations;

namespace Inventra.Controllers
{
    public class WarehouseLocationsController : Controller
    {
        private readonly InventraDbContext _context;

        public WarehouseLocationsController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: WarehouseLocations
        public async Task<IActionResult> Index()
        {
            var locations = await _context.WarehouseLocations
                .Select(l=>new WarehouseLocationIndexViewModel
                {
                    WarehouseLocationId = l.WarehouseLocationId,
                    LocationCode = l.LocationCode,
                    Description = l.Description,
                    IsFull = l.IsFull
                })
                .ToListAsync();

            return View(locations);
        }

        // GET: WarehouseLocations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var location = await _context.WarehouseLocations
                .Where(l=>l.WarehouseLocationId==id)
                .Select(l=>new WarehouseLocationDetailsViewModel
                {
                    WarehouseLocationId = l.WarehouseLocationId,
                    LocationCode = l.LocationCode,
                    Description = l.Description,
                    IsFull = l.IsFull
                })
                .FirstOrDefaultAsync();

            if (location==null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: WarehouseLocations/Create
        public IActionResult Create()
        {
            return View(new WarehouseLocationCreateViewModel());
        }

        // POST: WarehouseLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WarehouseLocationCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var warehouseLocation = new WarehouseLocation
            {
                WarehouseLocationId = Guid.NewGuid(),
                LocationCode = model.LocationCode,
                Description = model.Description,
                IsFull = false
            };

            await _context.WarehouseLocations.AddAsync(warehouseLocation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: WarehouseLocations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var warehouseLocation = await _context.WarehouseLocations.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            var model = new WarehouseLocationEditViewModel
            {
                WarehouseLocationId = warehouseLocation.WarehouseLocationId,
                LocationCode = warehouseLocation.LocationCode,
                Description = warehouseLocation.Description,
                IsFull = warehouseLocation.IsFull
            };

            if(warehouseLocation == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: WarehouseLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WarehouseLocationIndexViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var warehouseLocation = await _context.WarehouseLocations.FindAsync(model.WarehouseLocationId);

            if (warehouseLocation == null)
            {
                return NotFound();
            }

            warehouseLocation.LocationCode = model.LocationCode;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: WarehouseLocations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseLocation = await _context.WarehouseLocations
                .FirstOrDefaultAsync(m => m.WarehouseLocationId == id);
            if (warehouseLocation == null)
            {
                return NotFound();
            }

            return View(warehouseLocation);
        }

        // POST: WarehouseLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var warehouseLocation = await _context.WarehouseLocations.FindAsync(id);
            if (warehouseLocation != null)
            {
                _context.WarehouseLocations.Remove(warehouseLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseLocationExists(Guid id)
        {
            return _context.WarehouseLocations.Any(e => e.WarehouseLocationId == id);
        }
    }
}
