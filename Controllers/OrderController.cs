using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
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

        public IActionResult Index()
        {
            var orders = _orderRepo.GetAll();

            var orderList = orders.Select(o => new OrderVM
            {
                PkId = o.PkId,
                FkUserId = o.FkUserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                //OrderDetails = o.OrderDetails,
            }).ToList();

            return View(orderList);
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

