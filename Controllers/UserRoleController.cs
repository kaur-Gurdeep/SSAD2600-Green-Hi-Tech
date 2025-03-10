using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GreenHiTech.Data;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using GreenHiTech.Models;

namespace DeckMaster.Controllers
{

    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityUserRepo _identityUserRepo;

        public UserRoleController(ApplicationDbContext context
                                 , UserManager<IdentityUser> userManager
                                 , RoleManager<IdentityRole> roleManager
                                 )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityUserRepo = new IdentityUserRepo(userManager);
        }
        public ActionResult Index()
        {
            var users = _identityUserRepo.GetUsers(); 
            return View(users);
        }

        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            var roles = await userRoleRepo.GetUserRolesAsync(userName);

            ViewBag.UserName = userName;

            return View(roles);
        }

        public ActionResult Create(string userName)
        {
            ViewBag.SelectedUser = userName;

            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                    .ToList();
            var roleList = new SelectList(preRoleList, "Value", "Text");

            ViewBag.RoleSelectList = roleList;

            var userList = _context.Users.ToList();

            var preUserList = userList.Select(u =>
                new SelectListItem { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList
                                                        , "Value"
                                                        , "Text");
            ViewBag.UserSelectList = userSelectList;
            return View();
        }        

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRoleAsync(userRoleVM.Email
                                                               , userRoleVM.Role);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                        new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        //[ActionName("Delete")]
        public async Task<IActionResult> DeleteRole(string userName, string roleName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            var result = await userRoleRepo.RemoveUserRoleAsync(userName, roleName);

            if (result)
            {
                TempData["SuccessMessage"] = "Role deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error while deleting the role.";
            }

            return RedirectToAction("Detail", new { userName });
        }
    }
}
