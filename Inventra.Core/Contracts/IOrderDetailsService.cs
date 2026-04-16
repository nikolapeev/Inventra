using Inventra.Core.ViewModels.OrderDetails;
using Inventra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface IOrderDetailsService
    {
        Task CreateAsync(OrderDetailsCreateViewModel model);

        Task DeleteAsync(Guid orderId, Guid productId);

        Task<OrderDetails?> GetByIdAsync(Guid orderId, Guid productId);

        Task UpdateAsync(OrderDetailsEditViewModel model);

        public Task DeleteOneAsync(Guid orderId, Guid productId);

        Task<List<Product>> GetAllProducts();
        Task<List<Order>> GetAllOrders();
    }
}
