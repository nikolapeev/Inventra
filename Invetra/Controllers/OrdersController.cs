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

namespace Inventra.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InventraDbContext _context;

        public OrdersController(InventraDbContext context)
        {
            _context = context;
        }
        ///
        /// 
        ///
        /// 
        //TRANSFER TO ORDER SERVICE METHOD BELOW
        ///
        /// 
        ///
        ///

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
                    AdditionalInfo = o.AdditionalInfo
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
                    TotalPrice = /*add method to service- sum each orderDetail associated with the order,*/,
                    AdditionalInfo= o.AdditionalInfo
                }).FirstOrDefaultAsync();

            if (order == null)
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

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId= model.CustomerId,
                CourierId=model.CourierId,
                TrackingNumber=model.TrackingNumber,
                TotalPrice=/* Add sum method in the servce*/,
                AdditionalInfo= model.AdditionalInfo

            };

            var couriers = _context.Couriers.ToListAsync();
            var customers = _context.Customers.ToListAsync();

            ViewBag.CourierId = new SelectList(await couriers, "Id", "Name");
            ViewBag.CustomerId = new SelectList(await customers, "Id", "Name");
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderIndexViewModel
            {
                Id = order.Id,
                CourierName = order.Courier.Name,
                TrackingNumber = order.TrackingNumber,
                TotalPrice = /*add method in the service; look at the lines above for context*/ ,
                AdditionalInfo = order.AdditionalInfo
            };

            return View(model);
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

            model.CourierName = order.Courier.Name;
            model.TrackingNumber=order.TrackingNumber;
            model.TotalPrice = order.TotalPrice;
            model.AdditionalInfo = order.AdditionalInfo;    

            await _context.SaveChangesAsync();

            return View(model);
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
