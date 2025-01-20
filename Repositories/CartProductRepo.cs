using GreenHiTech.Data;
using GreenHiTech.Models;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Repositories
{
    public class CartProductRepo
    {
        private readonly ApplicationDbContext _context;

        public CartProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CartProductVM> GetAll(int userPkId)
        {
            IEnumerable<CartProductVM> cartProducts = _context.cartProducts
                .Where(cp => cp.FkCartId == userPkId)
                .Select(cp => new CartProductVM
                {
                    PkId = cp.PkId,
                    FkProductId = cp.FkProductId,
                    Quantity = cp.Quantity
                }).ToList();
            return cartProducts;
        }

        public void Delete(int cartProductId)
        {
            var cartProduct = _context.cartProducts.Find(cartProductId);
            if (cartProduct != null)
            {
                _context.cartProducts.Remove(cartProduct);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cart Product not found");
            }
        }

        public void AddToCart(int userPkId, int productId, int quantity)
        {
            var cartProduct = new CartProduct
            {
                FkCartId = userPkId,
                FkProductId = productId,
                Quantity = quantity
            };
            _context.cartProducts.Add(cartProduct);
            _context.SaveChanges();
        }

        // Update cart product
        public string Update(CartProduct cartProduct)
        {
            if (Any(cartProduct.PkId))
            {
                try
                {
                    _context.cartProducts.Update(cartProduct);
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

        // if cart product exists
        public bool Any(int id)
        {
            return _context.cartProducts.Any(cp => cp.PkId == id);
        }
    }
}
