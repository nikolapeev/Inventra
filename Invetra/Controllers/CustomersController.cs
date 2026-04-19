using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Customers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);

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

            await _customerService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);

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
                CompanyName=customer.CompanyName,
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

            await _customerService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
