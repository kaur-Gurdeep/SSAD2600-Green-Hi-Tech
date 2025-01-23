using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using System.Net;

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
            return View();
        }

        public IActionResult Details(int id)
        {
            var user = _userRepo.GetById(id);

            if (user == null)
            {
                _logger.LogWarning("Unable to find User ID: {Id}", id);
                return RedirectToAction("Index", new
                {
                    message = $"warning,Unable to find User Id: {id}"
                });
            }
            var address = _addressDetailRepo.GetById(user.FkAddressId);
            var userVM = new UserVM
            {
                PkUserId = user.PkId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                //Phone = user.Phone,
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
            var address = _addressDetailRepo.GetById(user.FkAddressId);
            var userVM = new UserVM
            {
                PkUserId = user.PkId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
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
            if (ModelState.IsValid)
            {
                var user = _userRepo.GetById(userVM.PkUserId);
                if (user == null) 
                {
                    string returnMessage = $"error,User could not be updated: (Email {userVM.Email})";
                    return RedirectToAction("Index", new { message = returnMessage });
                }

                user.Email = userVM.Email;
                user.FirstName = userVM.FirstName;
                user.LastName = userVM.LastName;
                //user.Phone = userVM.Phone;
                user.Role = userVM.Role;

                string result = _userRepo.Update(user);
                if (result.StartsWith("error"))
                {
                    string returnMessage = $"error,Failed to update User: (Email {user.Email})";
                    return RedirectToAction("Index", new { message = returnMessage });
                }

                if (userVM.AddressDetail != null)
                {
                    var address = _addressDetailRepo.GetById(user.FkAddressId) ?? new AddressDetail();

                    //address.PkId = 0;
                    address.Unit = userVM.AddressDetail.Unit;
                    address.HouseNumber = userVM.AddressDetail.HouseNumber;
                    address.Street = userVM.AddressDetail.Street;
                    address.City = userVM.AddressDetail.City;
                    address.Province = userVM.AddressDetail.Province;
                    address.PostalCode = userVM.AddressDetail.PostalCode;
                    address.Country = userVM.AddressDetail.Country;

                    //string addressResult = address.PkId == 0 ? _addressDetailRepo.Add(address) : _addressDetailRepo.Update(address);
                    string addressResult = _addressDetailRepo.Add(address);


                    if (addressResult.StartsWith("error"))
                    {
                        string returnMessage = $"error,Failed to update Address: (User Email {user.Email})";
                        return RedirectToAction("Index", new { message = returnMessage });
                    }

                    //user.FkAddressID = address.PkId;
                    //_userRepo.Update(user);
                }  

            }
            return View(userVM);

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
