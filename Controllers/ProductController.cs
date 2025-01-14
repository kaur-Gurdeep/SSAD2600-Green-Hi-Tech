using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Controllers
{
    public class ProductController : Controller
    {
        // Repo methods: Add, GetAll, GetById, Update, Delete

        private readonly ProductRepository _productRepo;

        public ProductController(ProductRepositories productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            var products = _productsRepo.GetAll();

            var productVMs = products.(p => new ProductVM
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Manufacturer = p.Manufacturer,
            }
            );

            return View(productVMs);
        }
    }
}
