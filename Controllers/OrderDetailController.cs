using GreenHiTech.Data;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
