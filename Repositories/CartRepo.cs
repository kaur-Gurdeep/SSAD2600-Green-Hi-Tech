using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class CartRepo
    {
        private readonly GreenHiTechContext _context;
        public CartRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all carts
        public List<Cart> GetAll()
        {
            return _context.Carts.ToList();
        }

        // Get cart by id
        public Cart? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.Carts.Any(c => c.PkId == id))
            {
                return null;
            }
            return _context.Carts.Find(id);
        }

        // Add cart
        public string Add(Cart cart)
        {
            try
            {
                _context.Carts.Add(cart);
                _context.SaveChanges();

                return $"success,Successfully created cart ID: {cart.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create cart: {e.Message}";
            }
        }

        // Update cart
        public string Update(Cart cart)
        {
            if (Any(cart.PkId))
            {
                try
                {
                    _context.Carts.Update(cart);
                    _context.SaveChanges();

                    return $"success,Successfully updated cart ID: {cart.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Cart could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find cart ID: {cart.PkId}";
            }
        }

        // Delete cart
        public string Delete(int id)
        {
            Cart? cart = GetById(id);
            if (cart == null)
            {
                return $"warning,Unable to find cart ID: {id}";
            }

            try
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return $"success,Successfully deleted cart ID: {id}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete cart: {e.Message}";
            }
        }

        // if cart exists
        public bool Any(int id)
        {
            return _context.Carts.Any(c => c.PkId == id);
        }
    }
}
