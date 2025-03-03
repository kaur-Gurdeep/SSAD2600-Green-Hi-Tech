using System.Diagnostics;
using GreenHiTech.Models;
using GreenHiTech.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductRepo _productReop;

        public HomeController(ILogger<HomeController> logger, ProductRepo productReop)
        {
            _logger = logger;
            _productReop = productReop;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
