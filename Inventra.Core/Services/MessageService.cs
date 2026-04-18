using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class MessageService : IMessageService
    {
        public Task<MessageCreateViewModel> CreateAsync(MessageCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MessageIndexViewModel>> GetAllMessages()
        {
            throw new NotImplementedException();
        }
    }
}
