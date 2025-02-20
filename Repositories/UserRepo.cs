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

//using GreenHiTech.Models;
//using GreenHiTech.ViewModels;
//using Microsoft.EntityFrameworkCore;

//namespace GreenHiTech.Repositories
//{
//    public class UserRepo
//    {
//        private readonly GreenHiTechContext _context;
//        private readonly AddressDetailRepo _addressDetailRepo;
//        private readonly UserRoleRepo _userRoleRepo;

//        public UserRepo(GreenHiTechContext context, AddressDetailRepo addressDetailRepo, UserRoleRepo userRoleRepo)
//        {
//            _context = context;
//            _addressDetailRepo = addressDetailRepo;
//            _userRoleRepo = userRoleRepo;
//        }

//        public List<UserVM> GetAll()
//        {
//            return _context.Users.Select(user => new UserVM
//            {
//                PkUserId = user.PkId,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                Role = user.Role,
//                Phone = user.Phone
//            }).ToList();
//        }

//        public UserVM? GetById(int id)
//        {
//            var user = _context.Users.Find(id);
//            if (user == null) return null;

//            var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0);
//            var roles = _userRoleRepo.GetUserRolesAsync(user.Email).Result;

//            return new UserVM
//            {
//                PkUserId = user.PkId,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                Role = user.Role,
//                Phone = user.Phone,
//                AddressDetail = address != null ? new AddressDetailVM
//                {
//                    PkId = address.PkId,
//                    Unit = address.Unit,
//                    HouseNumber = address.HouseNumber,
//                    Street = address.Street,
//                    City = address.City,
//                    PostalCode = address.PostalCode,
//                    Country = address.Country,
//                    Province = address.Province
//                } : null,
//                RoleList = roles.ToList(),
//            };
//        }

//        // Add user
//        public string Add(GreenHiTech.Models.User user)
//        {
//            try
//            {
//                _context.Users.Add(user);
//                _context.SaveChanges();
//                return $"success,Successfully created user ID: " +
//                       $"{user.PkId}";
//            }
//            catch (Exception e)
//            {
//                return $"error,Failed to create user: {e.Message}";
//            }
//        }
//        public string UpdateUser(UserVM userVM)
//        {
//            var user = _context.Users.Find(userVM.PkUserId);
//            if (user == null) return $"error,User ID: {userVM.PkUserId} not found";

//            user.Email = userVM.Email;
//            user.FirstName = userVM.FirstName;
//            user.LastName = userVM.LastName;
//            user.Phone = userVM.Phone;
//            user.Role = userVM.Role;

//            _context.Users.Update(user);
//            _context.SaveChanges();

//            if (userVM.AddressDetail != null)
//            {
//                var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0) ?? new AddressDetail
//                {
//                    Unit = userVM.AddressDetail.Unit,
//                    HouseNumber = userVM.AddressDetail.HouseNumber,
//                    Street = userVM.AddressDetail.Street,
//                    City = userVM.AddressDetail.City,
//                    Province = userVM.AddressDetail.Province,
//                    PostalCode = userVM.AddressDetail.PostalCode,
//                    Country = userVM.AddressDetail.Country
//                };

//                if (address.PkId == 0)
//                    _addressDetailRepo.Add(address);
//                else
//                    _addressDetailRepo.Update(address);

//                user.FkAddressId = address.PkId;
//                _context.Users.Update(user);
//                _context.SaveChanges();
//            }

//            return $"success,Successfully updated User: (Email {user.Email})";
//        }

//        //Delete user
//                public string Delete(int id)
//        {
//            if (Any(id))
//            {
//                try
//                {
//                    _context.Users.Remove(_context.Users.Find(id));
//                    _context.SaveChanges();
//                    return $"success,Successfully deleted user ID: {id}";
//                }
//                catch (Exception e)
//                {
//                    return $"error,Failed to delete user: {e.Message}";
//                }
//            }
//            return $"error,User ID: {id} not found";
//        }

//        // Check if user exists
//        public bool Any(int id)
//        {
//            return _context.Users.Any(u => u.PkId == id);
//        }
//    }
//}

