using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Orders;
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
            // You MUST project into the ViewModel here
            var orders = await _context.Orders
                .Select(o => new OrderIndexViewModel
                {
                    Id = o.Id,
                    CustomerName = o.Customer.FullName,
                    CourierName = o.Courier.Name,
                    TrackingNumber = o.TrackingNumber,
                    TotalPrice = o.TotalPrice
                }).ToListAsync();

            // Pass the 'orders' (which is a List of OrderIndexViewModel) to the View
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            // 1. Grab the main Order from the database
            var order = await _context.Orders
                // .Include(o => o.Customer) // Uncomment this if you have a Customer linked!
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();

            // 2. 🟢 THE MISSING PIECE: Grab all OrderDetails for this specific order
            var orderItems = await _context.OrderDetails
                .Include(od => od.Product) // We MUST Include the Product so we can see the Name!
                .Where(od => od.OrderId == id)
                .ToListAsync();

            // 3. Pack everything into your ViewModel
            var viewModel = new Inventra.Models.Orders.OrderDetailsViewModel
            {
                Id = order.Id,
                CustomerName = order.Customer?.FullName, // Adjust to match your exact properties
                TrackingNumber = order.TrackingNumber,
                AdditionalInfo=order.AdditionalInfo,
                TotalPrice = order.TotalPrice,
                // 🟢 Pass the list we just fetched into the ViewModel!
                Products = orderItems
            };

            return View(viewModel);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            // Load dropdowns
            ViewBag.CustomerId = new SelectList(await _context.Customers.OrderBy(x => x.FullName).ToListAsync(), "CustomerId", "FullName");
            ViewBag.CourierId = new SelectList(await _context.Couriers.OrderBy(x => x.Name).ToListAsync(), "CourierId", "Name");

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
                ViewBag.CustomerId = new SelectList(await _context.Customers.OrderBy(x => x.FullName).ToListAsync(), "CustomerId", "FullName", model.CustomerId);
                ViewBag.CourierId = new SelectList(await _context.Couriers.OrderBy(x => x.Name).ToListAsync(), "CourierId", "Name", model.CourierId);
                return View(model);
            }

             

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = model.CustomerId,
                CourierId = model.CourierId,
                TrackingNumber = model.TrackingNumber,
                AdditionalInfo = model.AdditionalInfo,
                TotalPrice = model.TotalPrice // Replace with Service method later
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            
            ViewBag.CustomerId = new SelectList(await _context.Customers.OrderBy(x => x.FullName).ToListAsync(), "CustomerId", "FullName", order.CustomerId);
            ViewBag.CourierId = new SelectList(await _context.Couriers.OrderBy(x => x.Name).ToListAsync(), "CourierId", "Name", order.CourierId);

            var model = new OrderIndexViewModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId, 
                CourierId = order.CourierId,   
                TrackingNumber = order.TrackingNumber,
                TotalPrice = order.TotalPrice,
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
                ViewBag.CustomerId = new SelectList(await _context.Customers.OrderBy(x => x.FullName).ToListAsync(), "CustomerId", "FullName", model.CustomerId);
                ViewBag.CourierId = new SelectList(await _context.Couriers.OrderBy(x => x.Name).ToListAsync(), "CourierId", "Name", model.CourierId);
                return View(model);
            }
            

            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null) return NotFound();

            // Map properties from ViewModel back to the Database Entity
            order.CustomerId = model.CustomerId;
            order.CourierId = model.CourierId;
            order.TrackingNumber = model.TrackingNumber;
            order.AdditionalInfo = model.AdditionalInfo;
            order.TotalPrice = model.TotalPrice;

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id)) return NotFound();
                else throw;
            }

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
