using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class CartProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
