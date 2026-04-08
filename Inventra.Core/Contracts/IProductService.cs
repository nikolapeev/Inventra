using Inventra.Core.ViewModels.Categories;
using Inventra.Core.ViewModels.Products;
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

        Task<ProductDetailsViewModel?> GetByIdAsync(Guid id);

        Task CreateAsync(ProductCreateViewModel model);

        Task UpdateAsync(ProductIndexViewModel model);

        Task DeleteAsync(Guid id);
    }
}
