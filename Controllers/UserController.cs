using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace GreenHiTech.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly AddressDetailRepo _addressDetailRepo;

        public UserController(ILogger<UserController> logger, UserRepo userRepo, AddressDetailRepo addressDetailRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
            _addressDetailRepo = addressDetailRepo;
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

        public IActionResult Detail(int id)
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
                Phone= user.Phone,
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
