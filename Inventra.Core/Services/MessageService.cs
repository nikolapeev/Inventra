using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Customers;
using Inventra.Core.ViewModels.Messages;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly InventraDbContext _context;

        public MessageService(InventraDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(MessageCreateViewModel model)
        {
            var message = new Message
            {
                Id = model.Id,
                Content = model.Content,
                CreatedBy=model.CreatedBy,
                Type= model.Type
            };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var message= await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return;
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageIndexViewModel>> GetAllAsync()
        {
            return await _context.Messages
                .Select(m => new MessageIndexViewModel
                {
                    Id = m.Id,
                    Content = m.Content,
                    CreatedBy = m.CreatedBy,
                    Type = m.Type,
                    CreatedAt=m.CreatedAt
                }).ToListAsync();
        }
    }
}
