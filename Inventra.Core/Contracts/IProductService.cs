using Inventra.Core.ViewModels.Categories;
using Inventra.Core.ViewModels.Products;
using Inventra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface IProductService
    {
        Task<List<ProductIndexViewModel>> GetAllAsync();

        Task<ProductDetailsViewModel?> GetDetailsByIdAsync(Guid id);

        Task CreateAsync(ProductCreateViewModel model , string? currentUserName);

        Task UpdateAsync(ProductEditViewModel model);

        Task DeleteAsync(Guid id);

        Task<List<Supplier>> ListSupplier();

        Task<List<Category>> ListCategory();

        Task<Product?> GetByIdAsync(Guid id);
    }
}
