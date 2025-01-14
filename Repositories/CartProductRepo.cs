﻿using GreenHiTech.Data;
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

        public IEnumerable<CartProductVM> GetAll(/*int userPkId*/)
        {
            IEnumerable<CartProductVM> cartProducts = _context.cartProducts
                //.Where(cp => cp.FkCartId == userPkId)
                .Select(cp => new CartProductVM
            {
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
    }
}
