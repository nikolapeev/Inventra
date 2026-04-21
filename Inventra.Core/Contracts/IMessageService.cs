using Inventra.Core.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface IMessageService
    {
        Task<List<MessageIndexViewModel>> GetAllAsync();

        Task CreateAsync(MessageCreateViewModel model);

        Task DeleteAsync(Guid id);
    }
}
