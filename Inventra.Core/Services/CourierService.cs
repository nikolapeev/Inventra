using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Couriers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class CourierService : ICourierService
    {
        public Task CreateAsync(CourierCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourierIndexViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CourierIndexViewModel> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CourierIndexViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
