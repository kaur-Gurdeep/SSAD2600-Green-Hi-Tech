using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using GreenHiTech.Models;

namespace GreenHiTech.Controllers
{
    public class ProductController : Controller
    {
        // Repo methods: Add, GetAll, GetById, Update, Delete

        private readonly ProductRepository _productRepo;

        public ProductController(ProductRepository productRepo)
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

        public IActionResult Details(int id)
        {
            var product = _productRepo.GetById(id);

            ProductVM productVM = new ProductVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Manufacturer = product.Manufacturer,
            };

            return View(productVM);
        }

        // GET
        public IActionResult Create()
        {
            return View(new ProductVM());
        }

        // POST
        [HttpPost]
        // manager access
        public IActionResult Create(ProductVM productVM)
        {
            string returnMessage = string.Empty;

            if(ModelState.IsValid)
            {
                try
                {
                    _productRepo.Add(productVM);
                    returnMessage = $"success,Successfully created Product: (Name {productVM.Name})";
                }
                catch (Exception ex)
                {
                    returnMessage = $"error,Product could not be created: (Name {productVM.Name})";
                }
            }
            else
            {
                return View(productVM);
            }

            return RedirectToAction("Index", new { message = returnMessage});
        }

        // GET
        public IActionResult Edit(int id)
        {
            Product? product = _productRepo.GetById(id);

            if (product == null)
            {
                return RedirectToAction("Index", new { message = $"warning,Product not found: (ID {id})" });
            }
            else
            {
                ProductVM productVM = new ProductVM
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    FkCategoryId = product.FkCategoryId,
                    Manufacturer = product.Manufacturer,
                };
                return View(productVM);
            }
        }

        // POST
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

        // POST
        [HttpPost, ActionName("Delete")]
        // manager access
        public IActionResult DeleteConfirm(int id)
        {
            return View();
        }
    }
}
