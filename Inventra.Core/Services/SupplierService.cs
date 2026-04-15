using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Suppliers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventraDbContext context;

        public SupplierService(InventraDbContext context)
        {
            this.context=context;
        }
        public async Task CreateAsync(SupplierCreateViewModel model)
        {
            var supplier = new Supplier
            {
                SupplierId = Guid.NewGuid(),
                Name = model.Name,
                EIK = model.EIK,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var supplier = await context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return;
            }

            context.Suppliers.Remove(supplier);
            await context.SaveChangesAsync();
        }

        public async Task<List<SupplierIndexViewModel>> GetAllSuppliers()
        {
            return await context.Suppliers
                .Select(s => new SupplierIndexViewModel
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    EIK = s.EIK,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email
                })
                .ToListAsync();
        }

        public async Task<Supplier> GetByIdAsync(Guid id) 
        {
           return await context.Suppliers.FindAsync(id);
        }

        public async Task UpdateAsync(SupplierEditViewModel model)
        {
            var supplier = await context.Suppliers.FindAsync(model.SupplierId);

            if (supplier == null)
            {
                return;
            }

            supplier.Name = model.Name;
            supplier.PhoneNumber = model.PhoneNumber;
            supplier.Email = model.Email;
            supplier.EIK = model.EIK;

            await context.SaveChangesAsync();
        }
    }
}
