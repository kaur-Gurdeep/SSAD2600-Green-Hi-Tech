using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GreenHiTech.Controllers
{

    public class RoleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleRepo _roleRepo;

        public RoleController(ILogger<HomeController> logger,
                              RoleRepo roleRepo)
        {
            _logger = logger;
            _roleRepo = roleRepo;
        }

        public IActionResult Index()
        {
            List<RoleVM> roleVM = _roleRepo.GetAllRoles();
            return View(roleVM);
        }

        public IActionResult Detail(string roleId)
        {
            var role = _roleRepo.GetRole(roleId); 
            if (role == null)
            {
                return NotFound(); 
            }
            return View(role); 
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = _roleRepo.CreateRole(roleVM.RoleName);

                if (isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Role creation failed." +
                                             " The role may already exist.");
                }
            }
            return View(roleVM);
        }

        [HttpPost]
        public IActionResult Delete(string roleId)
        {
            string errorMessage;
            bool isSuccess = _roleRepo.DeleteRole(roleId, out errorMessage); 

            if (isSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", errorMessage);
                return View("Index", _roleRepo.GetAllRoles());
            }
        }

    }
}
