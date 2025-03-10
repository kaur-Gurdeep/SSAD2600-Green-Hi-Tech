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
        private readonly CategoryRepo _categoryRepo;
        private readonly ProductImageRepo _productImageRepo;

        public HomeController(ILogger<HomeController> logger, ProductRepo productReop, CategoryRepo categoryRepo, ProductImageRepo productImageRepo)
        {
            _logger = logger;
            _productReop = productReop;
            _categoryRepo = categoryRepo;
            _productImageRepo = productImageRepo;
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

            var category = _categoryRepo.GetById(categoryID);
            ViewBag.CategoryName = "in "+category?.Name ?? "";


            foreach (Product product in productsInCategory)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                List<ProductImageVM> productImageVMs = new List<ProductImageVM>();
                ProductImage? firstProductImage = _productImageRepo.GetFirstForProductId(product.PkId);

                if(firstProductImage != null)
                {
                    productImages.Add(firstProductImage);
                
                    ProductImageVM productImageVM = new ProductImageVM
                    {
                        PkId = productImages[0].PkId,
                        AltText = productImages[0].AltText,
                        FkProductId = productImages[0].FkProductId,
                        ImageUrl = productImages[0].ImageUrl,
                        CreateDate = productImages[0].CreateDate,
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

        public IActionResult About()
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
