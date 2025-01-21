using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class OrderController: Controller
    {
        private readonly OrderRepo _orderRepo;

        public OrderController(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
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


        //public IActionResult Create() { 
        //}


    }
}
