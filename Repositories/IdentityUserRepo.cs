using Microsoft.AspNetCore.Identity;
using GreenHiTech.Data;
using GreenHiTech.ViewModels;
using GreenHiTech.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenHiTech.Repositories
{
    public class IdentityUserRepo
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityUserRepo(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        public List<IdentityUserVM> GetUsers()
        {
            var users = _userManager.Users.Select(u => new IdentityUserVM { Email = u.Email }).ToList();
            return users;
        }

        //public async Task AddUser(User customUser)
        //{
        //    if (customUser != null)
        //    {
        //        _context.Users.Add(customUser);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}