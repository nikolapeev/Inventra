using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Orders;
using Inventra.Models.Categories;

namespace Inventra.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InventraDbContext _context;

        public OrdersController(InventraDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Select(o => new OrderIndexViewModel
                {
                    Id = o.Id,
                    CustomerName = o.Customer.FullName,
                    CourierName = o.Courier.Name,
                    TrackingNumber = o.TrackingNumber,
                    TotalPrice = o.TotalPrice,
                }).ToListAsync();

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsViewModel
                {
                    Id = o.Id,
                    CustomerName = o.Customer.FullName,
                    CourierName = o.Courier.Name,
                    TrackingNumber = o.TrackingNumber,
                    TotalPrice = o.TotalPrice,

                })
                .FirstOrDefaultAsync();

            if ( order==null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View(new OrderCreateViewModel());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = model.CustomerId,
                CourierId = model.CourierId,
                TrackingNumber = model.TrackingNumber,
                TotalPrice = model.TotalPrice
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            if (order == null)
            {
                return NotFound();
            }
            
            var model = new OrderEditViewModel()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CourierId = order.CourierId,
                TrackingNumber = order.TrackingNumber,
                TotalPrice = order.TotalPrice
            };

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderIndexViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);    
            }

            var order = await _context.Orders.FindAsync(model.Id);

            if (order == null)
            {
                return NotFound();
            }

            order.TrackingNumber = model.TrackingNumber;

            await _context.SaveChangesAsync();  


            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
