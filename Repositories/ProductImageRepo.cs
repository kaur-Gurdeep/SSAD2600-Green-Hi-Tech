using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class ProductImageRepo
    {
        private readonly GreenHiTechContext _context;
        public ProductImageRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all product images   
        public List<ProductImage> GetAll()
        {
            return _context.ProductImages.ToList();
        }

        // Get product image by id
        public ProductImage? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.ProductImages.Any(p => p.PkId == id))
            {
                return null;
            }
            return _context.ProductImages.Find(id);
        }

        // Add product image
        public string Add(ProductImage productImage)
        {
            try
            {
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
                return $"success,Successfully created product image ID: " +
                       $"{productImage.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create product image: {e.Message}";
            }
        }

        // Update product image
        public string Update(ProductImage productImage)
        {
            if (Any(productImage.PkId))
            {
                try
                {
                    _context.ProductImages.Update(productImage);
                    _context.SaveChanges();
                    return $"success,Successfully updated product image ID: {productImage.PkId}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to update product image: {e.Message}";
                }
            }
            return $"error,Product image ID: {productImage.PkId} does not exist";
        }

        // Check if product image exists
        public bool Any(int id)
        {
            return _context.ProductImages.Any(p => p.PkId == id);
        }

        // Delete product image
        public string Delete(int id)
        {
            ProductImage? productImage = _context.ProductImages.Find(id);
            if (productImage != null) {
                try
                {
                    _context.ProductImages.Remove(productImage);
                    _context.SaveChanges();
                    return $"success,Successfully deleted product image ID: {id}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to delete product image: {e.Message}";
                }
            }
            return $"error,Product image ID: {id} does not exist";
        }
           
    }
}
