using Inventra.Core.ViewModels.Categories;
using Inventra.Core.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface ICustomerService
    {
        Task<List<CustomerIndexViewModel>> GetAllAsync();

        Task<CustomerDetailsViewModel?> GetByIdAsync(Guid id);

        Task CreateAsync(CustomerCreateViewModel model);

        Task UpdateAsync(CustomerIndexViewModel model);

        Task DeleteAsync(Guid id);
    }
}
