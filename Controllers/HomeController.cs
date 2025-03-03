using System.Diagnostics;
using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
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

        public IActionResult Category(int categoryID)
        {
            List<ProductVM> productVMs = new List<ProductVM>();
            List<Product> productsInCategory = new List<Product>();
            productsInCategory = _productReop.GetByCategory(categoryID);

            foreach (Product product in productsInCategory)
            {
                List<ProductImageVM> productImageVMs = new List<ProductImageVM>();
                foreach (ProductImage productImage in product.ProductImages)
                {
                    ProductImageVM productImageVM = new ProductImageVM
                    {
                        PkId = productImage.PkId,
                        AltText = productImage.AltText,
                        FkProductId = productImage.FkProductId,
                        ImageUrl = productImage.ImageUrl,
                        CreateDate = productImage.CreateDate,
                    };
                    productImageVMs.Add(productImageVM);
                }

                ProductVM productVM = new ProductVM
                {
                    PkId = product.PkId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    FkCategoryId = product.FkCategoryId,
                    Manufacturer = product.Manufacturer,
                    ProductImageVMs = productImageVMs,
                };

                productVMs.Add(productVM);
            }

            return View("../Product/Index", productVMs);
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
