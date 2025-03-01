using GreenHiTech.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenHiTech.Repositories
{
    public class UserRepo
    {
        private readonly GreenHiTechContext _context;

        public UserRepo(GreenHiTechContext context)
        {
            _context = context;
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

        // Delete user
        public string Delete(int id)
        {
            if (Any(id))
            {
                try
                {
                    _context.Users.Remove(_context.Users.Find(id));
                    _context.SaveChanges();
                    return $"success,Successfully deleted user ID: {id}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to delete user: {e.Message}";
                }
            }
            return $"error,User ID: {id} not found";
        }

        // Check if user exists
        public bool Any(int id)
        {
            return _context.Users.Any(u => u.PkId == id);
        }

    }
}

