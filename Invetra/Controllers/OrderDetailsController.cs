using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.OrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    public class OrderDetailsController:Controller
    {
        private readonly InventraDbContext _context;

        public OrderDetailsController(InventraDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == id)
                .Select(od => new OrderDetailsDetailsViewModel
                {
                    ProductName = od.Product.Name,
                    QTY = od.QTY,
                    Subtotal =  od.Product.Price * od.QTY 
                })
                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new OrderDetailsCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDetailsCreateViewModel model)
        {

            var desiredProduct = await _context.Products.FindAsync(model.ProductId);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var orderDetail = new OrderDetails
            {
                OrderId = model.OrderId,
                ProductId = model.ProductId,
                QTY = model.QTY,
                Subtotal = desiredProduct.Price* model.QTY

            };

            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return View(nameof(Index));
        }

        //providing the ORDER id 
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var detail=await _context.OrderDetails.FindAsync(id);


            if (detail == null)
            {
                return NotFound();
            }

            var model = new OrderDetails
            {
                OrderId=detail.OrderId,
                ProductId = detail.ProductId,
                QTY = detail.QTY,
                Subtotal =detail.QTY*de 
            }
        }
}
