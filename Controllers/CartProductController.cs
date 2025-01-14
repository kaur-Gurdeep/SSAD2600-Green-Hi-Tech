using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class CartProductController : Controller
    {
        private readonly CartProductRepo _cartProductRepo;

        public CartProductController(CartProductRepo cartProductRepo)
        {
            _cartProductRepo = cartProductRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<CartProductVM> cartProducts = _cartProductRepo.GetAll();
            return View();
        }
    }
}
