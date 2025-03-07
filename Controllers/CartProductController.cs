using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GreenHiTech.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace GreenHiTech.Controllers
{
    [Authorize]
    public class CartProductController : Controller
    {
        
        private readonly CartProductRepo _cartProductRepo;
        private readonly GreenHiTechContext _context;
        private readonly OrderRepo _orderRepo;
        private readonly OrderDetailRepo _orderDetailRepo;
        private readonly PaymentRepo _paymentRepo;

        public CartProductController(CartProductRepo cartProductRepo, GreenHiTechContext context, OrderRepo orderRepo, OrderDetailRepo orderDetailRepo, PaymentRepo paymentRepo)
        {
            _cartProductRepo = cartProductRepo;
            _context = context;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _paymentRepo = paymentRepo;
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

        public IActionResult Payment()
        {
            int userPkId = GetUserPkId();

            decimal totalAmount = _cartProductRepo.GetTotalAmount(userPkId);
            decimal taxTotal = _cartProductRepo.GetTaxTotal(userPkId);
            decimal subTotal = _cartProductRepo.GetSubTotal(userPkId);
            ViewBag.TaxTotal = taxTotal.ToString("f2");
            ViewBag.SubTotal = subTotal.ToString("f2");
            ViewBag.TotalAmount = totalAmount.ToString("f2");

            //STYLE CHECKOUT PAGE
            return View();
        }

        public IActionResult PaymentConfirmation(string confirmationId)
        {
            int userId = GetUserPkId();

            decimal totalAmount = _cartProductRepo.GetTotalAmount(userId);
            Order order = new Order()
            {
                FkUserId = userId,
                TotalAmount = totalAmount,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending"
            };
            var newOrder = _orderRepo.Add(order);
            int orderId = 0;
            var match = Regex.Match(newOrder, @"ID: (\d+)");
            if (match.Success)
            {
                orderId = int.Parse(match.Groups[1].Value);
            }


            Payment payment = new Payment()
            {
                FkOderId = orderId,
                PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                Amount = _cartProductRepo.GetTotalAmount(userId),
                TransactionId = 1 // change database to accept confirmationId
            };
            var newPayment = _paymentRepo.Add(payment);

            IEnumerable<CartProductVM> CartItems = _cartProductRepo.GetAll(userId);


            foreach(var cartItem in CartItems)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    FkOrderId = orderId,
                    FkProductId = cartItem.FkProductId,
                    Quantity = cartItem.Quantity,
                };

                var newOrderDetail = _orderDetailRepo.Add(orderDetail);
                
            }

            //Clear the cart



            //DISPLAYS EMAIL/ TRANSACTION ID/ AMOUNT PAID

            //PASS THIS TO VIEW



            ViewBag.confirmationId = confirmationId;
            return View();
        }

        private int GetUserPkId()
        {
            var userEmail = User.Identity.Name;
            var userPkId = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return userPkId.PkId;
        }

        private User GetUser(int userPkId)
        {
            return _context.Users.Include(u => u.FkAddress).FirstOrDefault(u => u.PkId == userPkId);
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

        [HttpGet]
        public IActionResult Checkout()
        {
            int userPkId = GetUserPkId();
            var user = GetUser(userPkId);
            ViewBag.Name = new List<string> {
            $"{user.FirstName} {user.LastName}"
            };
            

            if (string.IsNullOrEmpty(user.FkAddress.HouseNumber) ||
                string.IsNullOrEmpty(user.FkAddress.Street) ||
                string.IsNullOrEmpty(user.FkAddress.City) ||
                string.IsNullOrEmpty(user.FkAddress.Province) ||
                string.IsNullOrEmpty(user.FkAddress.Country))
            {
                ViewBag.AddressIncomplete = true;
            }
            else
            {
                ViewBag.AddressIncomplete = false;
                ViewBag.Address = new List<string>
                {
                    $"{user.FkAddress.HouseNumber} {user.FkAddress.Street}, {user.FkAddress.City}, {user.FkAddress.Province}, {user.FkAddress.Country}"
                };
            }

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
        public IActionResult Checkout(string Address)
        {
            int userPkId = GetUserPkId();
            var user = GetUser(userPkId);
            var addressComponents = Address.Split(',');
            user.FkAddress.HouseNumber = addressComponents[0].Split(' ')[0];
            user.FkAddress.Street = addressComponents[0].Split(' ')[1];
            user.FkAddress.City = addressComponents[1].Trim();
            user.FkAddress.Province = addressComponents[2].Trim();
            user.FkAddress.Country = addressComponents[3].Trim();
            _context.SaveChanges();

            ViewBag.Name = $"{user.FirstName} {user.LastName}";
            ViewBag.Address = new List<string>
            {
                $"{user.FkAddress.HouseNumber} {user.FkAddress.Street}, {user.FkAddress.City}, {user.FkAddress.Province}, {user.FkAddress.Country}"
            };
            IEnumerable<CartProductVM> cartProducts = _cartProductRepo.GetAll(userPkId);
            decimal totalAmount = _cartProductRepo.GetTotalAmount(userPkId);
            decimal taxTotal = _cartProductRepo.GetTaxTotal(userPkId);
            decimal subTotal = _cartProductRepo.GetSubTotal(userPkId);
            ViewBag.TaxTotal = taxTotal.ToString("f2");
            ViewBag.SubTotal = subTotal.ToString("f2");
            ViewBag.TotalAmount = totalAmount.ToString("f2");
            return View(cartProducts);
        }
    }
}
