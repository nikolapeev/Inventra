using Inventra.Core.ViewModels.Orders;
using Inventra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface IOrderService
    {
        public Task<List<OrderIndexViewModel>> GetAllOrders();

        public Task<OrderDetailsViewModel> GetDetailsById(Guid id);

        public Task<List<Customer>> GetCustomerList();
        public Task<List<Courier>> GetCourierList();

        public Task CreateAsync(OrderCreateViewModel model);

        public Task UpdateAsync(OrderIndexViewModel model);

        public Task DeleteAsync(Guid id);

        public Task<Order?> GetOrderById(Guid id);

        public bool OrderExists(Guid id);     
    }
}
