using Microsoft.AspNetCore.Identity;
using GreenHiTech.Data;
using GreenHiTech.ViewModels;
using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class IdentityUserRepo
    {
        private readonly ApplicationDbContext _context;

        public IdentityUserRepo(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<IdentityUserVM> GetUsers()
        {
            var users = _context.Users.Select(u => new IdentityUserVM { Email = u.Email }).ToList();
            return users;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
