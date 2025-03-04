using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class StaffDashboardController : Controller
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;
        private readonly ProductImageRepo _productImageRepo;
        private readonly OrderRepo _orderRepo;

        public StaffDashboardController(ProductRepo productRepo, CategoryRepo categoryRepo, ProductImageRepo productImageRepo, OrderRepo orderRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _productImageRepo = productImageRepo;
            _orderRepo = orderRepo;
        }


        public IActionResult Index()
        {
            decimal totalSales = 0m;
            int totalOrders = _orderRepo.GetAll().Count;
            _orderRepo.GetAll().ForEach(o => totalSales += o.TotalAmount);
            decimal averageSales = totalSales / totalOrders;

            ViewBag.TotalSales = totalSales;
            ViewBag.AverageSales = averageSales;
            ViewBag.TotalOrders = totalOrders;

            List<OrderVM> orderVMs = new List<OrderVM>();
            var orders = _orderRepo.GetAll();

            orders.ForEach(o => orderVMs.Add(new OrderVM
            {
                PkId = o.PkId,
                FkUserId = o.FkUserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
            }));

            return View(orderVMs);
        }
    }
}
