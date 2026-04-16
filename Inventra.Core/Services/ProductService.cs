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
        public async Task CreateAsync(ProductCreateViewModel model, string? currentUserName)
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
                AddedBy = currentUserName ?? "System"
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product= await context.Products.FindAsync(id);
            if (product == null)
            {
                return;
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Supplier>> ListSupplier()
        {
            return await context.Suppliers.OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<List<Category>> ListCategory()
        {
            return await context.Categories.OrderBy(s => s.Name).ToListAsync();
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

        public async Task<ProductDetailsViewModel?> GetDetailsByIdAsync(Guid id)
        {
            return await context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    SupplierName= p.Supplier.Name,
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

        public async Task UpdateAsync(ProductEditViewModel model)
        {
            var product = await context.Products.FindAsync(model.Id);

            if (product == null)
            {
                return ;
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.SupplierId = model.SupplierId;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;
            product.ImageURL = model.ImageURL;
            product.BatchNumber = model.BatchNumber;
            product.WarehouseLocationId = model.WarehouseLocationId;

            await context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await context.Products.FindAsync(id);
        }
    }
}
