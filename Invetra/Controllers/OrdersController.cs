using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Orders;
using Inventra.Data;
using Inventra.Data.Entities;
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
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            //if (id == null) return NotFound();

            
            //var order = await _context.Orders
            //    // .Include(o => o.Customer) // Uncomment this if you have a Customer linked!
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (order == null) return NotFound();

            
            //var orderItems = await _context.OrderDetails
            //    .Include(od => od.Product) 
            //    .Where(od => od.OrderId == id)
            //    .ToListAsync();

     
            //var viewModel = new Inventra.Core.ViewModels.Orders.OrderDetailsViewModel
            //{
            //    Id = order.Id,
            //    CustomerName = order.Customer?.FullName,
            //    TrackingNumber = order.TrackingNumber,
            //    AdditionalInfo=order.AdditionalInfo,
            //    TotalPrice = order.TotalPrice,
            //    Products = orderItems
            //};

            var order = await _orderService.GetDetailsById(id);

            return View(order);
            //return View(viewModel);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            // Load dropdowns
            ViewBag.CustomerId = new SelectList(await _orderService.GetCustomerList(), "CustomerId", "FullName");
            ViewBag.CourierId = new SelectList(await _orderService.GetCourierList(), "CourierId", "Name");

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
                ViewBag.CustomerId = new SelectList(await _orderService.GetCustomerList(), "CustomerId", "FullName", model.CustomerId);
                ViewBag.CourierId = new SelectList(await _orderService.GetCourierList(), "CourierId", "Name", model.CourierId);
                return View(model);
            }

            await _orderService.CreateAsync(model);

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
                ViewBag.CustomerId = new SelectList(await _orderService.GetCustomerList(), "CustomerId", "FullName", model.CustomerId);
                ViewBag.CourierId = new SelectList(await _orderService.GetCourierList(), "CourierId", "Name", model.CourierId);
                return View(model);
            }


            //var order = await _context.Orders.FindAsync(model.Id);
            //if (order == null) return NotFound();

            //order.CustomerId = model.CustomerId;
            //order.CourierId = model.CourierId;
            //order.TrackingNumber = model.TrackingNumber;
            //order.AdditionalInfo = model.AdditionalInfo;
            //order.TotalPrice = model.TotalPrice;

            //try
            //{
            //    _context.Update(order);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!OrderExists(order.Id)) return NotFound();
            //    else throw;
            //}

            await _orderService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _orderService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
