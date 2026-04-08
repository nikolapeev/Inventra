using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Customers;
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
    public class CustomerService : ICustomerService
    {

        private readonly InventraDbContext context;

        public CustomerService(InventraDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(CustomerCreateViewModel model)
        {
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
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return;
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }

        public async Task<List<CustomerIndexViewModel>> GetAllAsync()
        {
            return await context.Customers
                .Select(c => new CustomerIndexViewModel
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

                }).ToListAsync();
        }

        public async Task<CustomerDetailsViewModel?> GetByIdAsync(Guid id)
        {
            return await context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerDetailsViewModel
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
        }

        public async Task UpdateAsync(CustomerIndexViewModel model)
        {
            var customer = await context.Customers.FindAsync(model.CustomerId);

            if (customer == null)
            {
                return;
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

            await context.SaveChangesAsync();
        }
    }
}
