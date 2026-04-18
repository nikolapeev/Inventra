using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Suppliers;
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
    [Authorize(Roles = "Administrator")]
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllSuppliers();

            return View(suppliers);
        }

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

            await _supplierService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);

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
        public async Task<IActionResult> Edit(SupplierEditViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }   

            await _supplierService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supplierService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
