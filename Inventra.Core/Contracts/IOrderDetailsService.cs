using Inventra.Core.ViewModels.OrderDetails;
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
    }
}
