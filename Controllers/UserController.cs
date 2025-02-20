using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Threading.Tasks;

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


        public IActionResult Index()
        {
            //if (User.IsInRole("Admin"))
            //{
            var users = _userRepo.GetAll();
            var userVMs = users.Select(user => new UserVM
            {
                PkUserId = user.PkId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone
            }).ToList();
            return View("Index", userVMs);
            //}
            //else
            //{
            //    return View();
            //}
        }

        public async Task<IActionResult> Detail(int id)
        {
            User? user = _userRepo.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning, Unable to find User Id: {id}"
                });
            }
            else

            {
                var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0);

                var roles = await _userRoleRepo.GetUserRolesAsync(user.Email);
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
        public IActionResult Edit(int id)
        {
            var user = _userRepo.GetById(id);
            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning, Unable to find User Id: {id}"
                });
            }
            var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0);
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
                } : null
            };

            return View(userVM);
        }
        [HttpPost]
        public IActionResult Edit(UserVM userVM)
        {
            string returnMessage = string.Empty;


            ModelState.Remove("RoleList");//will remove this field when fixed the rolefunctionality

            if (ModelState.IsValid)
            {
                var user = _userRepo.GetById(userVM.PkUserId);
                if (user == null)
                {
                    returnMessage = $"error,User could not be updated: (Email {userVM.Email})";
                    return RedirectToAction("Index", new { message = returnMessage });
                }

                user.Email = userVM.Email;
                user.FirstName = userVM.FirstName;
                user.LastName = userVM.LastName;
                user.Phone = userVM.Phone;
                user.Role = userVM.Role;

                string result = _userRepo.Update(user);
                if (result.StartsWith("error"))
                {
                    returnMessage = $"error,Failed to update User: (Email {user.Email})";
                    return RedirectToAction("Index", new { message = returnMessage });
                }

                if (userVM.AddressDetail != null)
                {
                    var address = _addressDetailRepo.GetById(user.FkAddressId ?? 0) ?? new AddressDetail();

                    address.Unit = userVM.AddressDetail.Unit;
                    address.HouseNumber = userVM.AddressDetail.HouseNumber;
                    address.Street = userVM.AddressDetail.Street;
                    address.City = userVM.AddressDetail.City;
                    address.Province = userVM.AddressDetail.Province;
                    address.PostalCode = userVM.AddressDetail.PostalCode;
                    address.Country = userVM.AddressDetail.Country;

                    string addressResult = address.PkId == 0 ? _addressDetailRepo.Add(address) : _addressDetailRepo.Update(address);


                    if (addressResult.StartsWith("error"))
                    {
                        returnMessage = $"error,Failed to update Address: (User Email {user.Email})";
                        return RedirectToAction("Index", new { message = returnMessage });
                    }

                    user.FkAddressId = address.PkId;
                    _userRepo.Update(user);
                    returnMessage = $"success,Successfully updated User: (User Email {user.Email})";

                    // Set success message in TempData
                    TempData["SuccessMessage"] = "Edited Successfully";
                }

            }
            else
            {
                // If there was a validation error
                returnMessage = "error,An unexpected error occurred while updating the user.";
                return RedirectToAction("Index", new { message = returnMessage });
            }

            return RedirectToAction("Edit", new { id = userVM.PkUserId });

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
    }
}


//using GreenHiTech.ViewModels;
//using GreenHiTech.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace GreenHiTech.Controllers
//{
//    public class UserController : Controller
//    {
//        private readonly UserRepo _userRepo;
//        private readonly ILogger<UserController> _logger;

//        public UserController(ILogger<UserController> logger, UserRepo userRepo)
//        {
//            _logger = logger;
//            _userRepo = userRepo;
//        }

//        public IActionResult Index()
//        {
//            var userVMs = _userRepo.GetAll();
//            return View("Index", userVMs);
//        }

//        public IActionResult Detail(int id)
//        {
//            var userVM = _userRepo.GetById(id);
//            if (userVM == null)
//            {
//                TempData["ErrorMessage"] = $"User ID {id} not found.";
//                return RedirectToAction("Index");
//            }
//            return View(userVM);
//        }

//        public IActionResult Edit(int id)
//        {
//            var userVM = _userRepo.GetById(id);
//            if (userVM == null)
//            {
//                TempData["ErrorMessage"] = $"User ID {id} not found.";
//                return RedirectToAction("Index");
//            }
//            return View(userVM);
//        }

//        [HttpPost]
//        public IActionResult Edit(UserVM userVM)
//        {
//            if (!ModelState.IsValid)
//            {
//                TempData["ErrorMessage"] = "Invalid data provided.";
//                return RedirectToAction("Edit", new { id = userVM.PkUserId });
//            }

//            string result = _userRepo.UpdateUser(userVM);

//            if (result.StartsWith("error"))
//            {
//                TempData["ErrorMessage"] = result;
//                return RedirectToAction("Edit", new { id = userVM.PkUserId });
//            }

//            TempData["SuccessMessage"] = "User updated successfully.";
//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public IActionResult Delete(int id)
//        {
//            if (id == 0)
//            {
//                return RedirectToAction("Index", new { message = "error, Invalid User ID." });
//            }

//            string result = _userRepo.Delete(id);

//            if (result.StartsWith("error"))
//            {
//                TempData["ErrorMessage"] = result;
//            }
//            else
//            {
//                TempData["SuccessMessage"] = "User deleted successfully.";
//            }

//            return RedirectToAction("Index");
//        }

//    }
//}
