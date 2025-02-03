using GreenHiTech.Data;
using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GreenHiTech.Repositories
{
    public class CartProductRepo
    {
        private readonly GreenHiTechContext _context;

        public CartProductRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        public IEnumerable<CartProductVM> GetAll(int userPkId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.FkUserId == userPkId);
            if (cart == null)
            {
                return Enumerable.Empty<CartProductVM>();
            }
            IEnumerable<CartProductVM> cartProducts = _context.CartProducts
                .Where(cp => cp.FkCartId == cart.PkId)
                .Select(cp => new CartProductVM
                {
                    PkId = cp.PkId,
                    FkProductId = cp.FkProductId,
                    Quantity = cp.Quantity,
                    Image = _context.ProductImages.FirstOrDefault(p => p.FkProductId == cp.FkProductId).ImageUrl,
                    ProductName = cp.FkProduct.Name,
                }).ToList();
            return cartProducts;
        }

        public void Delete(int cartProductId)
        {
            var cartProduct = _context.CartProducts.Find(cartProductId);
            if (cartProduct != null)
            {
                _context.CartProducts.Remove(cartProduct);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cart Product not found");
            }
        }

        public void AddToCart(int userPkId, int productId, int quantity)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.FkUserId == userPkId);
            if (cart == null)
            {
                cart = new Cart { FkUserId = userPkId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            var cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.FkCartId == cart.PkId && cp.FkProductId == productId);
            if (cartProduct != null)
            {
                cartProduct.Quantity += quantity;
            } 
            else 
            {
                cartProduct = new CartProduct
                {
                    FkCartId = cart.PkId,
                    FkProductId = productId,
                    Quantity = quantity
                };
                _context.CartProducts.Add(cartProduct);
            };
            _context.SaveChanges();
        }

        public void IncreaseQuantity(int cartProductId)
        {
            var cartProduct = _context.CartProducts.Find(cartProductId);
            if (cartProduct != null)
            {
                cartProduct.Quantity++;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cart Product not found");
            }
        }

        public void DecreaseQuantity(int cartProductId)
        {
            var cartProduct = _context.CartProducts.Find(cartProductId);
            if (cartProduct != null)
            {
                if (cartProduct.Quantity > 1)
                {
                    cartProduct.Quantity--;
                    _context.SaveChanges();
                }
                else
                {
                    Delete(cartProductId);
                }
            }
            else
            {
                throw new Exception("Cart Product not found");
            }
        } 

        public decimal GetSubTotal(int userPkId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.FkUserId == userPkId);
            if (cart == null)
            {
                return 0;
            }
            decimal subTotal = _context.CartProducts
                .Where(cp => cp.FkCartId == cart.PkId)
                .Sum(cp => cp.FkProduct.Price * cp.Quantity);
            return subTotal;
        }

        public decimal GetTaxTotal(int userPkId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.FkUserId == userPkId);
            if (cart == null)
            {
                return 0;
            }
            decimal taxTotal = _context.CartProducts
                .Where(cp => cp.FkCartId == cart.PkId)
                .Sum(cp => cp.FkProduct.Price * cp.Quantity * 0.12M);
            return taxTotal;
        }

        public decimal GetTotalAmount(int userPkId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.FkUserId == userPkId);
            if (cart == null)
            {
                return 0;
            }
            decimal totalAmount = _context.CartProducts
                .Where(cp => cp.FkCartId == cart.PkId)
                .Sum(cp => (cp.FkProduct.Price * cp.Quantity) + (cp.FkProduct.Price * cp.Quantity * 0.12M));
            return totalAmount;
        }
    }
}
