using Inventra.Core.Contracts;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventra.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly InventraDbContext context;

        public CategoryService(InventraDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(CategoryCreateViewModel model)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name= model.Name
                
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cat = await context.Categories.FindAsync(id);
            if (cat == null)
            {
                return;
            }

            context.Categories.Remove(cat);
            await context.SaveChangesAsync();
        }

        public async Task<List<CategoryIndexViewModel>> GetAllAsync()
        {
            return await context.Categories
                .Select(x => new CategoryIndexViewModel
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryIndexViewModel?> GetByIdAsync(Guid id)
        {
            return await context.Categories
                 .Where(x => x.CategoryId == id)
                 .Select(x => new CategoryIndexViewModel
                 {
                     CategoryId = x.CategoryId,
                     Name = x.Name
                 }).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(CategoryIndexViewModel model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                return;
            }
            category.Name = model.Name;

            await context.SaveChangesAsync();
        }
    }
}
