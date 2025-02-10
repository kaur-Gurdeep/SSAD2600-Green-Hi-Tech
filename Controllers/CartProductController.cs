using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GreenHiTech.Models;
using System.Text.RegularExpressions;

namespace GreenHiTech.Controllers
{
    [Authorize]
    public class CartProductController : Controller
    {
        
        private readonly CartProductRepo _cartProductRepo;
        private readonly GreenHiTechContext _context;

        public CartProductController(CartProductRepo cartProductRepo, GreenHiTechContext context)
        {
            _cartProductRepo = cartProductRepo;
            _context = context;
        }
        public IActionResult Index()
        {
            //var Users = _context.Users.ToList();
            //var x = 0;
            int userPkId = GetUserPkId();
            IEnumerable<CartProductVM> cartProducts = _cartProductRepo.GetAll(userPkId);
            decimal totalAmount = _cartProductRepo.GetTotalAmount(userPkId);
            decimal taxTotal = _cartProductRepo.GetTaxTotal(userPkId);
            decimal subTotal = _cartProductRepo.GetSubTotal(userPkId);
            ViewBag.TaxTotal = taxTotal.ToString("f2");
            ViewBag.SubTotal = subTotal.ToString("f2");
            ViewBag.TotalAmount = totalAmount.ToString("f2");
            return View(cartProducts);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            int userPkId = GetUserPkId();
            _cartProductRepo.AddToCart(userPkId, productId, quantity);
            return RedirectToAction("Index");
        }

        private int GetUserPkId()
        {
            var userEmail = User.Identity.Name;
            var userPkId = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return userPkId.PkId;
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
