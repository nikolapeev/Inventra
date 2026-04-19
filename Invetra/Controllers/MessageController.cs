using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Messages;
using Inventra.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace Inventra.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _messageService.GetAllAsync();
            return View(messages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Convert enum to a list of SelectListItems
            ViewBag.MessageTypes = Enum.GetValues<MessageType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();

            

            return View(new MessageCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string currentUsername = User.Identity?.Name;

            await _messageService.CreateAsync(model, currentUsername);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var cat= await _context.Categories.FindAsync(id);

            //if (cat == null)
            //{
            //    return NotFound();
            //}

            //_context.Categories.Remove(cat);
            //await _context.SaveChangesAsync();
            await _messageService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
