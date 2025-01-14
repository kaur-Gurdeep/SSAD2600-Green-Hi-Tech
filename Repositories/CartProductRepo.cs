using GreenHiTech.Data;
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
                PkId = cp.PkId,
                FkCartId = cp.FkCartId,
                FkProductId = cp.FkProductId,
                Quantity = cp.Quantity
            });
            return cartProducts;
        }
    }
}
