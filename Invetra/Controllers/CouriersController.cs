using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventra.Data;
using Inventra.Data.Entities;

namespace Inventra.Controllers
{
    public class CouriersController : Controller
    {
        private readonly InventraDbContext _context;

        public CouriersController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: Couriers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Couriers.ToListAsync());
        }

        // GET: Couriers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Couriers
                .FirstOrDefaultAsync(m => m.CourierId == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        // GET: Couriers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Couriers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourierId,Name")] Courier courier)
        {
            if (ModelState.IsValid)
            {
                courier.CourierId = Guid.NewGuid();
                _context.Add(courier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courier);
        }

        // GET: Couriers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Couriers.FindAsync(id);
            if (courier == null)
            {
                return NotFound();
            }
            return View(courier);
        }

        // POST: Couriers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourierId,Name")] Courier courier)
        {
            if (id != courier.CourierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourierExists(courier.CourierId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courier);
        }

        // GET: Couriers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Couriers
                .FirstOrDefaultAsync(m => m.CourierId == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        // POST: Couriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var courier = await _context.Couriers.FindAsync(id);
            if (courier != null)
            {
                _context.Couriers.Remove(courier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourierExists(Guid id)
        {
            return _context.Couriers.Any(e => e.CourierId == id);
        }
    }
}
