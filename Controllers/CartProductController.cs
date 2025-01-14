using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GreenHiTech.Controllers
{
    [Authorize]
    public class CartProductController : Controller
    {
        
        private readonly CartProductRepo _cartProductRepo;

        public CartProductController(CartProductRepo cartProductRepo)
        {
            _cartProductRepo = cartProductRepo;
        }
        public IActionResult Index()
        {
            int userPkId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            IEnumerable<CartProductVM> cartProducts = _cartProductRepo.GetAll(userPkId);
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            int userPkId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _cartProductRepo.AddToCart(userPkId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _cartProductRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
