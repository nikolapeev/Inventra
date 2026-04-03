using Inventra.Models.Categories;
using System;
using System.Collections.Generic;
using System.Text;


namespace Inventra.Core.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryIndexViewModel>> GetAllAsync();

        Task<CategoryIndexViewModel?> GetByIdAsync(Guid id);

        Task CreateAsync(CategoryCreateViewModel model);

        Task UpdateAsync(CategoryIndexViewModel model);

        Task DeleteAsync(Guid id);

    }
}
