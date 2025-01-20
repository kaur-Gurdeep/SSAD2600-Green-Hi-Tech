using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;

namespace GreenHiTech.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UserRepo userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
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

            var userView = GetUserView(user);
            return View(userView);
        }


        public IActionResult Edit(int id)
        {
            User user = _userRepo.GetById(id);
            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning, Unable to find User Id: {id}"
                });
            }

            // Only include Role if the user is ADMIN or STAFF
            string role = (user.Role == "ADMIN" || user.Role == "STAFF") ? user.Role : null;

            var userView = GetUserView(user, role);

            return View(userView);
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
