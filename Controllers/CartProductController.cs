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
            decimal totalAmount = _cartProductRepo.GetTotalAmount(userPkId);
            decimal taxTotal = _cartProductRepo.GetTaxTotal(userPkId);
            decimal subTotal = _cartProductRepo.GetSubTotal(userPkId);
            ViewBag.TaxTotal = taxTotal;
            ViewBag.SubTotal = subTotal;
            ViewBag.TotalAmount = totalAmount;
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

        [HttpPost]
        public IActionResult IncreaseQuantity(int id)
        {
            _cartProductRepo.IncreaseQuantity(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int id)
        {
            try
            {
                _cartProductRepo.DecreaseQuantity(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = e.Message });
            }
            return RedirectToAction("Index");
        }
    }
}
