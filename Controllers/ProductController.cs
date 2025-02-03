﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using GreenHiTech.Models;
using Microsoft.AspNetCore.Hosting;

namespace GreenHiTech.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepo _productRepo;
        private readonly CategoryRepo _categoryRepo;
        private readonly ProductImageRepo _productImageRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProductRepo productRepo, CategoryRepo categoryRepo, ProductImageRepo productImageRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _productImageRepo = productImageRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<ProductVM> productVMs = new List<ProductVM>();
            var products = _productRepo.GetAll();

            foreach(var product in products)
            {
                List<ProductImageVM> productImageVMs = new List<ProductImageVM>();

                List<ProductImage>  productImages = _productImageRepo.GetAll().Where(pi => pi.FkProductId == product.PkId).ToList();
                //if(productImages.Count > 0)
                //{
                    foreach(var productImage in productImages)
                    {
                        ProductImageVM productImageVM = new ProductImageVM
                        {
                            AltText = productImage.AltText,
                            FkProductId = productImage.FkProductId,
                            ImageUrl = productImage.ImageUrl,
                            CreateDate = productImage.CreateDate,
                        };
                        productImageVMs.Add(productImageVM);
                    }
                //}

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

            return View(productVMs);
        }

        public IActionResult Details(int id)
        {
            string returnMessage = string.Empty;
            Product? product = _productRepo.GetById(id);

            if(product == null) {
                returnMessage = $"error,Could not find product ID: {id}";
                return RedirectToAction("Index");
            }
            else
            {
                List<ProductImage> productImages = new List<ProductImage>();
                productImages = _productImageRepo.GetAll().Where(pi => pi.FkProductId == product.PkId).ToList();

                List<ProductImageVM> productImageVMs = new List<ProductImageVM>();
                foreach(var productImage in productImages)
                {
                    ProductImageVM productImageVM = new ProductImageVM
                    {
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
                return View(productVM);
            }
        }

        // GET
        public IActionResult Create()
        {
            string returnMessage = string.Empty;
            var categoryItems = _categoryRepo.GetAll().Select(c =>  new SelectListItem
            {
                Value = c.PkId.ToString(),
                Text = c.Name
            }).ToList();
            if(categoryItems == null)
            {
                returnMessage = "cannot find caterories";
                return RedirectToAction("Index", new { message = returnMessage });
            }
            else
            {
                SelectList categorySelectList = new SelectList(categoryItems, "Value", "Text");
                ViewBag.CategorySelectList = categorySelectList;
                return View(new ProductVM());
            }
        }

        // POST
        [HttpPost]
        // manager access
        public IActionResult Create(ProductVM productVM, List<IFormFile> fileImages)
        {
            string returnMessage = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product
                    {
                        Name = productVM.Name,
                        Description = productVM.Description,
                        Price = productVM.Price,
                        StockQuantity = productVM.StockQuantity,
                        FkCategoryId = productVM.FkCategoryId,
                        Manufacturer = productVM.Manufacturer,
                    };

                    _productRepo.Add(product);
                    int productId = _productRepo.GetAll().Max(p => p.PkId);

                    if(fileImages.Count > 0)
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string imageFolderPath = Path.Combine(webRootPath, "images", productId.ToString());

                        if(!Directory.Exists(imageFolderPath))
                        {
                            Directory.CreateDirectory(imageFolderPath);
                        }

                        //const string filepath = "/images/";
                        foreach(var fileImage in fileImages)
                        {
                            string fileName = Path.GetFileName(fileImage.FileName);
                            string filePath = Path.Combine(imageFolderPath, fileName);
                            using(var stream = new FileStream(filePath, FileMode.Create))
                            {
                                fileImage.CopyTo(stream);
                            }

                            ProductImage productImage = new ProductImage
                            {
                                AltText = $"{productId}_{product.Name}_",
                                FkProductId = productId,
                                ImageUrl = $"/images/{productId}/{fileName}",
                                CreateDate = DateOnly.FromDateTime(DateTime.Now),
                            };
                            _productImageRepo.Add(productImage);
                        }
                    }

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

            return RedirectToAction("Index", new { message = returnMessage });
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
                    PkId = product.PkId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    FkCategoryId = product.FkCategoryId,
                    Manufacturer = product.Manufacturer,
                    //ProductImages = product.ProductImages,
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

            return RedirectToAction("Index", new { message = returnMessage });
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
            if (product == null)
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
