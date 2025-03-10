using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepo _orderRepo;
        private readonly UserRepo _userRepo;

        public OrderController(OrderRepo orderRepo, UserRepo userRepo)
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string? userEmail = User.Identity.Name;
            List<Order>? orders = new List<Order>();
            User? user = _userRepo.GetByEmail(userEmail);
                if (user != null)
                {
                //var userVMs = await _userRepo.GetUsersWithRolesAsync();
                //var aspUser = userVMs.Select(u => u.PkUserId == user.PkId).FirstOrDefault();
                //var role = aspUser.Role;

                ////if (user.Role == "Admin" || user.Role == "Staff")
                //    {
                //        orders = _orderRepo.GetAll();
                //    } 
                //    else
                //    {
                        int userId = user.PkId;
                        orders = _orderRepo.GetByUserId(userId);
                    //}

                    List<OrderVM> orderVMs = new List<OrderVM>();
                    foreach(Order order in orders)
                    {
                        OrderVM orderVM = new OrderVM
                        {
                            PkId = order.PkId,
                            FkUserId = order.FkUserId,
                            OrderDate = order.OrderDate,
                            TotalAmount = order.TotalAmount,
                            Status = order.Status
                        };
                        orderVMs.Add(orderVM);
                    }
                    return View(orderVMs);
                }
            return View(null);
        }

        [HttpPost]
        public IActionResult Create(string transactionId, string amount, string status, string payerEmail)
        {
            if (ModelState.IsValid)
            {
                int userId = _userRepo.GetAll().Where(u => u.Email == payerEmail).FirstOrDefault().PkId;

                OrderVM newOrder = new OrderVM
                {
                    FkUserId = userId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    TotalAmount = decimal.Parse(amount),
                    Status = status,
                };

                // Map OrderVM to Order (data model)
                Order orderToAdd = new Order
                {
                    FkUserId = newOrder.FkUserId,
                    OrderDate = newOrder.OrderDate,
                    TotalAmount = newOrder.TotalAmount,
                    Status = newOrder.Status
                };

                string result = _orderRepo.Add(orderToAdd);
                if (result.StartsWith("success"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest(result);
                }

            }
            return BadRequest(ModelState);
        }
    }
}

