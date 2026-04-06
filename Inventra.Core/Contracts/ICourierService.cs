using Inventra.Core.ViewModels.Couriers;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface ICourierService
    {
        Task<IEnumerable<CourierIndexViewModel>> GetAllAsync();

        Task<CourierIndexViewModel> GetByIdAsync(Guid id);

        Task CreateAsync(CourierCreateViewModel model);

        Task UpdateAsync(CourierIndexViewModel model);

        Task DeleteAsync(Guid id);
    }
}
