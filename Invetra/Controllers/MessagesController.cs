using Inventra.Core.Contracts;
using Inventra.Core.ViewModels.Messages;
using Inventra.Data.Entities;
using Inventra.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Principal;

namespace Inventra.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
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
        public async Task<IActionResult> Create()
        {
            ViewBag.MessageTypes = Enum.GetValues<MessageType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();

            var userEmail = User.FindFirstValue(ClaimTypes.Name);

            MessageCreateViewModel model = new MessageCreateViewModel
            {
                CreatedBy = userEmail ?? "System"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string currentUsername = User.Identity.Name;
                model.CreatedBy = currentUsername;
            }


            await _messageService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _messageService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
