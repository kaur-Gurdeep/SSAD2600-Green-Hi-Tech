using GreenHiTech.Models;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Repositories
{
    public class ProductRepo
    {
        private readonly GreenHiTechContext _context;
        public ProductRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all products
        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public List<Product> Search(string searchTerm, List<ProductImage> allProductImages)
        {
            var products = _context.Products
                .Where(p => p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm) ||
                            p.Manufacturer.Contains(searchTerm))
                .ToList();

            foreach (var product in products)
            {
                product.ProductImages = allProductImages.Where(pi => pi.FkProductId == product.PkId).ToList();
            }

            return products;
        }

        // Get all products with ProductImages filled
        public List<Product> GetAll(List<ProductImage> allProductImages)
        {
            List<ProductVM> productVMs = new List<ProductVM>();
            List<ProductImage> productImages = new List<ProductImage>();
            List<Product> products = _context.Products.ToList();

            foreach(var product in products)
            {
                productImages = allProductImages.Where(pi => pi.FkProductId == product.PkId).ToList();
                product.ProductImages = productImages;
            }

            return products;
        }

        // Get product by id
        public Product? GetById(int id)
        {
            if(id == 0 || !_context.Products.Any(p => p.PkId == id))
            {
                return null;
            }
            else
            {
                return GetAll().Where(p => p.PkId == id).FirstOrDefault();
            }
        }

        // Get products by category
        public List<Product> GetByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.FkCategoryId == categoryId).ToList();
        }

        // Get product by id with ProductImages filled
        public Product? GetById(int id, List<ProductImage>? allProductImages)
        {
            if(id == 0 || !_context.Products.Any(p => p.PkId == id))
            {
                return null;
            }
            else
            {
                Product? product = GetAll().Where(p => p.PkId == id).FirstOrDefault();
                if(product == null)
                {
                    return null;
                } 
                else
                {
                    if(allProductImages == null)
                    {
                        product.ProductImages = new List<ProductImage>();
                    }
                    else
                    {
                        product.ProductImages = allProductImages.Where(pi => pi.FkProductId == id).ToList();
                    }
                    return product;
                }
            }
        }

        // Add product
        public string Add(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return $"success,Successfully created product ID: " +
                       $"{product.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create product: {e.Message}";
            }
        }

        // Update product
        public string Update(Product product)
        {
            if (Any(product.PkId))
            {
                try
                {
                    _context.Products.Update(product);
                    _context.SaveChanges();

                    return $"success,Successfully updated " +
                           $"product ID: {product.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Product could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find product ID: {product.PkId}";
            }
        }

        // Delete product
        public string Delete(Product product)
        {
            //Product? product = GetById(id);

            if (product == null)
            {
                return $"warning,Unable to find product ID: {product.PkId}";
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return $"success,Successfully deleted product ID: {product.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete product: {e.Message}";
            }
        }

        // if product exists
        public bool Any(int id)
        {
            return _context.Products.Any(p => p.PkId == id);
        }

    }
}
