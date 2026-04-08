using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Products;
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
    public class ProductService : IProductService
    {
        private readonly InventraDbContext context;

        public ProductService(InventraDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(ProductCreateViewModel model)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                ImageURL = model.ImageURL,
                BatchNumber = model.BatchNumber,
                WarehouseLocationId = model.WarehouseLocationId,
                AddedBy = User.Identity?.Name ?? "System" // Added this as your Entity marks it [Required]
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductIndexViewModel>> GetAllAsync()
        {
            return await context.Products
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    SupplierName = p.Supplier.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    BatchNumber = p.BatchNumber,
                    WarehouseLocationId = p.WarehouseLocationId
                }).ToListAsync();
        }

        public async Task<ProductDetailsViewModel?> GetByIdAsync(Guid id)
        {
            return await context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageURL = p.ImageURL,
                    BatchNumber = p.BatchNumber,
                    WarehouseLocationId = p.WarehouseLocationId,
                    AddedBy = p.AddedBy
                })
            .FirstOrDefaultAsync();
        }

        public Task UpdateAsync(ProductIndexViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
