using GreenHiTech.Models;

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

        // Get product by id
        public Product? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            } else if (!_context.Products.Any(p => p.PkId == id))
            {
                return null;
            }
            return _context.Products.Find(id);
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
        public string Delete(int id)
        {
            Product? product = GetById(id);
            if (product == null)
            {
                return $"warning,Unable to find product ID: {id}";
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return $"success,Successfully deleted product ID: {id}";
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
