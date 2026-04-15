using Inventra.Core.ViewModels.Categories;
using Inventra.Core.ViewModels.Suppliers;
using Inventra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface ISupplierService
    {
        Task<List<SupplierIndexViewModel>> GetAllSuppliers();

        Task<Supplier> GetByIdAsync(Guid id);

        Task CreateAsync(SupplierCreateViewModel model);

        Task UpdateAsync(SupplierEditViewModel model);

        Task DeleteAsync(Guid id);
    }
}
