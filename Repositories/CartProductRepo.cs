using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class CartProductRepo
    {
        private readonly GreenHiTechContext _context;
        public CartProductRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all cart products
        public List<CartProduct> GetAll()
        {
            return _context.CartProducts.ToList();
        }

        // Get cart product by id
        public CartProduct? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.CartProducts.Any(cp => cp.PkId == id))
            {
                return null;
            }
            return _context.CartProducts.Find(id);
        }

        // Add cart product
        public string Add(CartProduct cartProduct)
        {
            try
            {
                _context.CartProducts.Add(cartProduct);
                _context.SaveChanges();

                return $"success,Successfully created cart product ID: {cartProduct.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create cart product: {e.Message}";
            }
        }

        // Update cart product
        public string Update(CartProduct cartProduct)
        {
            if (Any(cartProduct.PkId))
            {
                try
                {
                    _context.CartProducts.Update(cartProduct);
                    _context.SaveChanges();

                    return $"success,Successfully updated cart product ID: {cartProduct.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Cart product could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find cart product ID: {cartProduct.PkId}";
            }
        }

        // Delete cart product
        public string Delete(int id)
        {
            CartProduct? cartProduct = GetById(id);
            if (cartProduct == null)
            {
                return $"warning,Unable to find cart product ID: {id}";
            }

            try
            {
                _context.CartProducts.Remove(cartProduct);
                _context.SaveChanges();
                return $"success,Successfully deleted cart product ID: {id}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete cart product: {e.Message}";
            }
        }

        // if cart product exists
        public bool Any(int id)
        {
            return _context.CartProducts.Any(cp => cp.PkId == id);
        }
    }
}
