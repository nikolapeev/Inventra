using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Couriers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class CourierService : ICourierService
    {
        private readonly InventraDbContext context;

        public CourierService(InventraDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CourierIndexViewModel>> GetAllAsync()
        {
            return await context.Couriers
                .Select(c => new CourierIndexViewModel
                {
                    CourierId = c.CourierId,
                    Name = c.Name,
                    Phone = c.Phone
                })
                .ToListAsync();
        }

        public async Task<CourierIndexViewModel> GetByIdAsync(Guid id)
        {
            return await context.Couriers
                 .Where(c => c.CourierId == id)
                 .Select(c => new CourierIndexViewModel
                 {
                     CourierId = c.CourierId,
                     Name = c.Name,
                     Phone = c.Phone
                 }).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CourierCreateViewModel model)
        {
            var courier = new Courier
            {
                CourierId = Guid.NewGuid(),
                Name = model.Name,
                Phone = model.Phone
            };

            await context.Couriers.AddAsync(courier);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourierIndexViewModel model)
        {
            var courier = await context.Couriers.FindAsync(model.CourierId);
            if (courier == null)
            {
                return;
            }

            courier.Name = model.Name;
            courier.Phone = model.Phone;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var courier = await context.Couriers.FindAsync(id);
            if (courier == null)
            {
                return;
            }

            context.Couriers.Remove(courier);
            await context.SaveChangesAsync();
        }
    }
}