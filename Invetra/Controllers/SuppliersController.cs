using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Suppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventra.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly InventraDbContext _context;

        public SuppliersController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Select(s=>new SupplierIndexViewModel
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    EIK = s.EIK,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email
                })
                .ToListAsync();

            return View(suppliers);
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Select(s => new SupplierDetailsViewModel
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    EIK = s.EIK,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email
                })
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View(new SupplierCreateViewModel());
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var supplier = new Supplier
            {
                SupplierId = Guid.NewGuid(),
                Name = model.Name,
                EIK = model.EIK,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }
            
            var model = new SupplierEditViewModel
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                EIK = supplier.EIK,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email
            };

            return View(model);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierIndexViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }   

            var supplier = await _context.Suppliers.FindAsync(model.SupplierId);    

            if(supplier == null)
            {
                return NotFound();
            }

            supplier.Name = model.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(Guid id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }
    }
}
