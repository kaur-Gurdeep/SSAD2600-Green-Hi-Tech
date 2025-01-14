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
            var products = _productRepo.GetAll();

            var productVMs = products.Where(p => new ProductVM
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Manufacturer = p.Manufacturer,
            });

            return View(productVMs);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // post
        [HttpPost]
        // manager access
        public IActionResult Create(ProductVM productVM)
        {
            return View();
        }

        // GET
        public IActionResult Edit(int id)
        {
            return View();
        }

        // post
        [HttpPost]
        // manager access
        public IActionResult Edit(ProductVM productVM)
        {
            return View();
        }

        // GET
        public IActionResult Delete(int id)
        {
            return View();
        }

        // post
        [HttpPost, ActionName("Delete")]
        // manager access
        public IActionResult DeleteConfirm(int id)
        {
            return View();
        }
    }
}
