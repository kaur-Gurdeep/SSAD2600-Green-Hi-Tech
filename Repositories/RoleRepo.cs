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

        public RoleVM GetRole(string roleName)
        {
            var role =
                _context.Roles.Where(r => r.Name == roleName)
                              .FirstOrDefault();

            if (role != null) {
                return new RoleVM() { RoleName = role.Name
                                    , Id = role.Id };
            }
            return null;
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
