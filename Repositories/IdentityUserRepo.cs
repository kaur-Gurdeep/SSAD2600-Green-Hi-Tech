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
        private readonly GreenHiTechContext _context;

        public IdentityUserRepo(UserManager<IdentityUser> userManager, GreenHiTechContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public List<IdentityUserVM> GetUsers()
        {
            var users = _context.Users.Select(u => new IdentityUserVM { Email = u.Email }).ToList();
            return users;
        }

        public async Task AddUser(User customUser)
        {
            if (customUser != null)
            {
                _context.Users.Add(customUser);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<User> GetUserByIdentityUserId(string identityUserId)
        //{
        //    return await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        //}
    }
}