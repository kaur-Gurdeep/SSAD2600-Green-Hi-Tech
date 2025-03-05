using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GreenHiTech.Repositories
{
    public class UserRepo
    {
        private readonly GreenHiTechContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserRoleRepo _userRoleRepo;

        public UserRepo(GreenHiTechContext context, UserManager<IdentityUser> userManager, UserRoleRepo userRoleRepo)
        {
            _context = context;
            _userManager = userManager;
            _userRoleRepo = userRoleRepo;
        }
        // Get all users
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        //Get user by id
        public User? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.Users.Any(u => u.PkId == id))
            {
                return null;
            }
            return _context.Users.Find(id);
        }

        public User? GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Add user
        public string Add(GreenHiTech.Models.User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return $"success,Successfully created user ID: " +
                       $"{user.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create user: {e.Message}";
            }
        }

        // Update user
        public string Update(User user)
        {
            if (Any(user.PkId))
            {
                try
                {
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return $"success,Successfully updated " +
                           $"user ID: {user.PkId}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to update user: {e.Message}";
                }
            }
            return $"error,User ID: {user.PkId} not found";
        }

        // Delete User

        //Only Admin can delete any user.
        //A regular user can delete their profile.
        public string Delete(int id)
        {
            try
            {
                var user = _context.Users.Find(id);

                if (user == null)
                {
                    return $"error, User ID: {id} not found";
                }

                // Find the IdentityUser by its email (assuming email is unique)
                var identityUser = _userManager.FindByEmailAsync(user.Email).Result;
                if (identityUser != null)
                {
                    // Get all roles of the user
                    var roles = _userManager.GetRolesAsync(identityUser).Result;

                    // Remove user from all roles
                    foreach (var role in roles)
                    {
                        _userManager.RemoveFromRoleAsync(identityUser, role).Wait();
                    }

                    // Delete the user from AspNetUsers
                    var identityResult = _userManager.DeleteAsync(identityUser).Result;
                    if (!identityResult.Succeeded)
                    {
                        return $"error, Failed to delete user from Identity: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}";
                    }
                }
                // Remove the user from the Users table
                _context.Users.Remove(user);
                _context.SaveChanges();

                return $"success, Successfully deleted user ID: {id}";
            }
            catch (Exception e)
            {
                return $"error, Failed to delete user: {e.Message}";
            }
        }

        public string GetFullNameByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return $"{user.FirstName} {user.LastName}";
            }
            return string.Empty; 
        }

        // Method to fetch users with their roles
        public async Task<List<UserVM>> GetUsersWithRolesAsync()
        {
            var users = _context.Users.ToList();
            var userVMs = new List<UserVM>();

            foreach (var user in users)
            {
                // Get the roles for the user asynchronously
                var roles = await _userRoleRepo.GetUserRolesAsync(user.Email);

                // Join the roles into a comma-separated string
                var roleString = string.Join(", ", roles.Select(role => role.RoleName));

                // Create the UserVM object and add it to the list
                userVMs.Add(new UserVM
                {
                    PkUserId = user.PkId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = roleString
                });
            }

            return userVMs;
        }

        // Check if user exists
        public bool Any(int id)
        {
            return _context.Users.Any(u => u.PkId == id);
        }

    }
}

