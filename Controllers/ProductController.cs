﻿using Microsoft.AspNetCore.Mvc;
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
                    PkId = id,
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
            string returnMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Product? product = _productRepo.GetById(productVM.PkId);
                if (product == null)
                {
                    returnMessage = $"error,Product could not be updated: (Name {productVM.Name})";
                } 
                else
                {
                    _productRepo.Update(product);
                    returnMessage = $"success,Successfully updated Product: (Name {product.Name})";
                }
            }

            return RedirectToAction("Index", new {message = returnMessage});
        }

        // GET
        public IActionResult Delete(int id)
        {
            Product? product = _productRepo.GetById(id);

            if (product == null)
            {
                return RedirectToAction("Index", new { message = $"warning,Product not found: (ID {id})" });
            }

            ProductVM productVM = new ProductVM
            {
                PkId = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                FkCategoryId = product.FkCategoryId,
                Manufacturer = product.Manufacturer,
            };

            ViewBag.ProductId = id;

            return View(productVM);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        // manager access
        public IActionResult DeleteConfirmed(int id)
        {
            Product? product = _productRepo.GetById(id);
            if(product == null)
            {
                return RedirectToAction("Index", new { message = $"warning,Product not found: (ID {id})" });
            }
            else
            {
                string returnMessage = _productRepo.Delete(id);
                return RedirectToAction("Index", new { message = returnMessage });
            }
        }
    }
}
