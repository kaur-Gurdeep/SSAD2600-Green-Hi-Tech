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
            IEnumerable<CartProductVM> cartProducts = _context.CartProducts
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
            var cartProduct = new CartProduct
            {
                FkCartId = userPkId,
                FkProductId = productId,
                Quantity = quantity
            };
            _context.CartProducts.Add(cartProduct);
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
            decimal subTotal = _context.CartProducts
                .Where(cp => cp.FkCartId == userPkId)
                .Sum(cp => cp.FkProduct.Price * cp.Quantity);
            return subTotal;
        }

        public decimal GetTaxTotal(int userPkId)
        {
            decimal taxTotal = _context.CartProducts
                .Where(cp => cp.FkCartId == userPkId)
                .Sum(cp => cp.FkProduct.Price * cp.Quantity * 0.12M);
            return taxTotal;
        }

        public decimal GetTotalAmount(int userPkId)
        {
            decimal totalAmount = _context.CartProducts
                .Where(cp => cp.FkCartId == userPkId)
                .Sum(cp => (cp.FkProduct.Price * cp.Quantity) + (cp.FkProduct.Price * cp.Quantity * 0.12M));
            return totalAmount;
        }
    }
}
