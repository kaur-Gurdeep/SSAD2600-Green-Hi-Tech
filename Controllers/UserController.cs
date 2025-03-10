using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GreenHiTech.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly AddressDetailRepo _addressDetailRepo;
        private readonly UserRoleRepo _userRoleRepo;

        public UserController(ILogger<UserController> logger, UserRepo userRepo, AddressDetailRepo addressDetailRepo, UserRoleRepo userRoleRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
            _addressDetailRepo = addressDetailRepo;
            _userRoleRepo = userRoleRepo;
        }

        public async Task<IActionResult> Index()
        {
            // Get users with roles from the UserRepo
            var userVMs = await _userRepo.GetUsersWithRolesAsync();

            // Return the Index view with the userVMs
            return View("Index", userVMs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // Fetch user by Id
            User? user = _userRepo.GetById(id);

            // If user is not found, redirect with warning message
            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning, Unable to find User Id: {id}"
                });
            }
            else
            {
                // Fetch address if FkAddressId is valid
                AddressDetail? address = null;
                if (user.FkAddressId.HasValue && user.FkAddressId.Value > 0)
                {
                    address = _addressDetailRepo.GetById(user.FkAddressId.Value);
                }

                // Get user roles asynchronously
                var roles = await _userRoleRepo.GetUserRolesAsync(user.Email);

                // Map to ViewModel
                var userVM = new UserVM
                {
                    PkUserId = user.PkId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Phone = user.Phone,
                    AddressDetail = address != null ? new AddressDetailVM
                    {
                        PkId = address.PkId,
                        Unit = address.Unit,
                        HouseNumber = address.HouseNumber,
                        Street = address.Street,
                        City = address.City,
                        PostalCode = address.PostalCode,
                        Country = address.Country,
                        Province = address.Province
                    } : null,
                    RoleList = roles.ToList(),
                };
                return View(userVM);
            }
        }

        public async Task<IActionResult> Edit(int id)  // Make method async
        {
            var user = _userRepo.GetById(id);  // Assuming GetById is synchronous, no need to await it
            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning, Unable to find User Id: {id}"
                });
            }

            // Await the asynchronous method to get the list of roles
            var roleList = await _userRoleRepo.GetUserRolesAsync(user.Email);

            var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0);

            // Creating ViewModel for the user and address
            var userVM = new UserVM
            {
                PkUserId = user.PkId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Phone = user.Phone,
                AddressDetail = address != null ? new AddressDetailVM
                {
                    PkId = address.PkId,
                    Unit = address.Unit,
                    HouseNumber = address.HouseNumber,
                    Street = address.Street,
                    City = address.City,
                    Province = address.Province,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                } : null,
                RoleList = roleList.ToList()  // Convert IEnumerable to List
            };

            // Check if the current user is an Admin to allow role editing
            var currentUserRole = User.IsInRole("Admin");
            if (!currentUserRole)
            {
                userVM.Role = null;  // If not an Admin, remove Role from ViewModel
            }
            else
            {
                userVM.Role = user.Role;  // If Admin, keep Role
            }

            return View(userVM);  // Return the View with the populated ViewModel
        }

        [HttpPost]
        public IActionResult Edit(UserVM userVM)
        {
            string returnMessage = string.Empty;

            ModelState.Remove("RoleList");

            if (ModelState.IsValid)  // Check if the model is valid
            {
                // Fetch the user by Id
                var user = _userRepo.GetById(userVM.PkUserId);
                if (user == null)
                {
                    returnMessage = $"error, User could not be updated Email {userVM.Email}";
                    TempData["Message"] = returnMessage;
                    return RedirectToAction("Index");
                }

                // If the current user is an Admin, allow the role change
                if (User.IsInRole("Admin"))
                {
                    user.Role = userVM.Role;  // Allow Admin to change the Role
                }

                // Update user details
                user.Email = userVM.Email;
                user.FirstName = userVM.FirstName;
                user.LastName = userVM.LastName;
                user.Phone = userVM.Phone;

                // Update user information in the repository
                string result = _userRepo.Update(user);
                if (result.StartsWith("error"))
                {
                    returnMessage = $"error, Failed to update User {user.Email}";
                    TempData["Message"] = returnMessage;
                    return RedirectToAction("Index");
                }

                // If the AddressDetail is provided, update the address
                if (userVM.AddressDetail != null)
                {
                    var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0) ?? new AddressDetail();

                    // Update address fields
                    address.Unit = userVM.AddressDetail.Unit;
                    address.HouseNumber = userVM.AddressDetail.HouseNumber!;
                    address.Street = userVM.AddressDetail.Street!;
                    address.City = userVM.AddressDetail.City!;
                    address.Province = userVM.AddressDetail.Province!;
                    address.PostalCode = userVM.AddressDetail.PostalCode!;
                    address.Country = userVM.AddressDetail.Country!;

                    // If address is new, add it; otherwise, update it
                    string addressResult = address.PkId == 0 ? _addressDetailRepo.Add(address) : _addressDetailRepo.Update(address);

                    // Update the user's address reference
                    user.FkAddressId = address.PkId;
                    _userRepo.Update(user);  // Update the user with new address

                    returnMessage = $"success, Successfully updated User: {user.Email}";

                    // Set success message in TempData
                    TempData["SuccessMessage"] = "Edited Successfully";
                }
            }

            // After successful POST, redirect to the appropriate page based on the role
            if (User.IsInRole("Admin"))
            {
                // Admin should be redirected to the Index page
                TempData["Message"] = returnMessage;
                return RedirectToAction("Index");
                //return RedirectToAction("Index", new { message = returnMessage });
            }
            else
            {
                // Customer (non-admin) should be redirected to the Details page
                return RedirectToAction("Detail", new { id = userVM.PkUserId });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _userRepo.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index", new { message = $"error, Unable to find User Id: {id}" });
            }

            string result = _userRepo.Delete(id);
            if (result.StartsWith("error"))
            {
                return RedirectToAction("Index", new { message = $"error, Failed to delete User Id: {id}" });
            }

            TempData["SuccessMessage"] = $"User Id: {id} has been deleted successfully.";

            // Check if the user is an admin
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index"); 
            }
            else
            {
                return RedirectToPage("/Account/Register");  
            }
        }

        private static UserVM GetUserView(User user, string? role = null)
        {
            return new UserVM
            {
                PkUserId = user.PkId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role ?? user.Role,
                Email = user.Email
            };
        }

        public IActionResult UserDashboard(string userEmail)
        {
            User? user = _userRepo.GetByEmail(userEmail);
            if (user != null)
            {
                UserVM userVM = new UserVM
                {
                    PkUserId = user.PkId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = "Customer",
                    Email = user.Email
                };

                return View(userVM);
            }
            else
            {
                return View();
            }
        }

    }
}


