using Microsoft.AspNetCore.Identity;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Repositories
{
    public class UserRoleRepo

    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleRepo(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        //Assign a role to a user.
        public async Task<bool> AddUserRoleAsync(string email
                                                , string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user
                                                                      , roleName);
                return result.Succeeded;
            }

            return false;
        }

        // Remove role from a user.
        public async Task<bool> RemoveUserRoleAsync(string email
                                                   , string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user
                                                                   , roleName);
                return result.Succeeded;
            }

            return false;
        }

        // Get all roles of a specific user.
        public async Task<IEnumerable<RoleVM>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.Select(roleName => new RoleVM { RoleName = roleName });
            }

            return Enumerable.Empty<RoleVM>();
        }
    }
}
