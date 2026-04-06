using Inventra.Core.ViewModels.Customers;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Categories;
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
    public class CustomersController : Controller
    {
        private readonly InventraDbContext _context;

        public CustomersController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Select(c=> new CustomerIndexViewModel
                {
                    CustomerId = c.CustomerId,
                    FullName=c.FullName,
                    PhoneNumber=c.PhoneNumber,
                    Email=c.Email,
                    Country=c.Country,
                    County=c.County,
                    City=c.City,
                    Address=c.Address,
                    PostalCode=c.PostalCode,
                    EIK=c.EIK,
                    ZDDS=c.ZDDS

                }).ToListAsync();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var customer=await _context.Customers
                .Where(c=>c.CustomerId==id)
                .Select(c=>new CustomerDetailsViewModel
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Country = c.Country,
                    County = c.County,
                    City = c.City,
                    Address = c.Address,
                    PostalCode = c.PostalCode,
                    EIK = c.EIK,
                    ZDDS = c.ZDDS
                }).FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();  
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View(new CustomerCreateViewModel());
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Country = model.Country,
                County = model.County,
                City = model.City,
                Address = model.Address,
                PostalCode = model.PostalCode,
                EIK = model.EIK,
                ZDDS = model.ZDDS

            };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) 
            { 
                return NotFound();  
            }

            var model = new CustomerIndexViewModel
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Country = customer.Country,
                County = customer.County,
                City = customer.City,
                Address = customer.Address,
                PostalCode = customer.PostalCode,
                EIK = customer.EIK,
                ZDDS = customer.ZDDS
            };
            
            return View(model);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = await _context.Customers.FindAsync(model.CustomerId);
            
            if (customer== null)
            {
                return NotFound();
            }

            customer.FullName = model.FullName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.Email = model.Email;
            customer.Country = model.Country;
            customer.County = model.County;
            customer.City = model.City;
            customer.Address = model.Address;
            customer.PostalCode = model.PostalCode;
            customer.EIK = model.EIK;
            customer.ZDDS = model.ZDDS;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
