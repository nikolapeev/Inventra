using Inventra.Data;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.Controllers
{
    public class CourierCountryController:Controller
    {
        private readonly InventraDbContext context;

        public CourierCountryController(InventraDbContext _context)
        {
            context= _context;
        }

        public async Task<IActionResult> Index()
        {
            return View();  
        }


    }
}
