using GreenHiTech.Models;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepo _userRepo;
        private readonly OrderController _logger;


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
            User user = _userRepo.GetById(id);
         

            if (user == null)
            {
                return RedirectToAction("Index", new
                {
                    message = $"warning,Unable to find User Id: {id}"
                });
            }

            var userView = new UserVM
            {
                pkUserId = user.PkId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Email = user.Email,


            };

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
            
            var userView = new UserVM
            {
                PkUserId = user.PkId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = _roleRepo.GetAll()
                        .Select(r => new SelectItem
            }
        }
    }
}
