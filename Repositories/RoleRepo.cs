using Microsoft.AspNetCore.Identity;
using GreenHiTech.Data;
using GreenHiTech.ViewModels;

namespace GreenHiTech.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _context;

        public RoleRepo(ApplicationDbContext context)
        {
            this._context = context;
            CreateInitialRole();
        }

        public List<RoleVM> GetAllRoles()
        {
            var roles = _context.Roles.Select(r => new RoleVM
                        {
                            Id = r.Id,
                            RoleName = r.Name
                        }).ToList();

            return roles;
        }

        public RoleVM GetRole(string roleId)
        {
            var role = _context.Roles
                .FirstOrDefault(r => r.Id == roleId); // Find the role by its ID

            if (role != null)
            {
                // Get a list of user emails assigned to this role
                var userEmails = _context.UserRoles
                    .Where(ur => ur.RoleId == role.Id)
                    .Join(_context.Users, ur => ur.UserId, u => u.Id, (ur, u) => u.Email) // Join to get the user's email
                    .ToList();

                // Return role details with users' emails
                return new RoleVM
                {
                    Id = role.Id,
                    RoleName = role.Name,
                    UsersCount = userEmails.Count, // Count of users assigned to this role
                    UserEmails = userEmails // List of user emails
                };
            }
            return null; // Return null if role not found
        }


        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _context.Roles.Add(new IdentityRole
                {
                    Name = roleName,
                    Id = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating role:" +
                                  $" {ex.Message}");

                isSuccess = false;
            }

            return isSuccess;
        }

        public bool DeleteRole(string roleId, out string errorMessage)
        {
            bool isSuccess = true;
            errorMessage = string.Empty;

            try
            {
                // Get the number of users assigned to this role
                var roleAssignedToUsersCount = _context.UserRoles.Count(ur => ur.RoleId == roleId);

                if (roleAssignedToUsersCount > 0)
                {
                    errorMessage = $"This role cannot be deleted because it is assigned to {roleAssignedToUsersCount} user(s).";
                    return false;
                }

                var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);

                if (role != null)
                {
                    _context.Roles.Remove(role);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting role: {ex.Message}");
                isSuccess = false;
            }

            return isSuccess;
        }



        public void CreateInitialRole()
        {
             const string ADMIN = "Admin";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN);
            }
        }
   }
}
