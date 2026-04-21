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
    [Authorize(Roles ="Administrator, OrderManager")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            
            var order = await _orderService.GetDetailsById(id);

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CustomerId = new SelectList(await _orderService.GetCustomerList(), "CustomerId", "FullName");
            ViewBag.CourierId = new SelectList(await _orderService.GetCourierList(), "CourierId", "Name");

            return View(new OrderCreateViewModel());
        }

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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null) return NotFound();
                
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();

            
            ViewBag.CustomerId = new SelectList(await _orderService.GetCustomerList(), "CustomerId", "FullName", order.CustomerId);
            ViewBag.CourierId = new SelectList(await _orderService.GetCourierList(), "CourierId", "Name", order.CourierId);

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
