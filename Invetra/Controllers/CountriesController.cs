using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models;
using Inventra.Models.Categories;
using Inventra.Models.Countries;

namespace Inventra.Controllers
{
    public class CountriesController : Controller
    {
        private readonly InventraDbContext _context;

        public CountriesController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            var countries = await _context.Countries
            .Select(c => new CountryIndexViewModel
            {
                CountryId = c.CountryId,
                Name = c.Name
            }).ToListAsync();
            return View(countries);

        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var country = await _context.Countries
                .Where(c => c.CountryId == id)
                .Select(c => new CountryIndexViewModel
                {
                    CountryId = c.CountryId,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View(new CountryCreateViewModel());
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryCreateViewModel model )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var country = new Country
            {
                CountryId = Guid.NewGuid(),
                Name = model.Name
            };

            await _context.Countries.AddAsync(country);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c=>c.CountryId==id);

            if (country == null)
            {
                return NotFound();
            }

            var model = new CountryIndexViewModel
            {
                CountryId = country.CountryId,
                Name=country.Name
            };

            return View(model);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CountryIndexViewModel model )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var country = await _context.Countries.FindAsync(model.CountryId);

            if (country == null)
            {
                return NotFound();
            }

            country.Name = model.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(Guid id)
        {
            return _context.Countries.Any(e => e.CountryId == id);
        }
    }
}
