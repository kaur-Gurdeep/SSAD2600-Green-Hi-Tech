using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GreenHiTech.Controllers
{
    //[Authorize(Roles = "Admin")]

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
    }
}
