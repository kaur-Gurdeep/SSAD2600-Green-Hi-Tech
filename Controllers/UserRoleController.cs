using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GreenHiTech.Data;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DeckMaster.Controllers
{

    //[Authorize(Roles = "Admin")]

    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleController(ApplicationDbContext context
                                 , UserManager<IdentityUser> userManager
                                 , RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            IdentityUserRepo userRepo = new IdentityUserRepo(_context);
            var users = userRepo.GetUsers();

            return View(users);
        }

        // Show all roles for a specific user.
        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            var roles = await userRoleRepo.GetUserRolesAsync(userName);

            ViewBag.UserName = userName;

            return View(roles);
        }

        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        public ActionResult Create(string userName)
        {
            // Store the email address of the Identity user
            // which is their user name.
            ViewBag.SelectedUser = userName;

            // Build SelectList with role data and store in ViewBag.
            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            // There might be a better way but I have always found using the 
            // .NET dropdown lists to be a challenge. Here is a way to make 
            // it work if you can get the data in the proper format. 

            // 1. Preparation for 'Roles' drop down.
            //    a) Build a list of SelectListItem objects which have 'Value' and 
            //       'Text' properties. 
            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                    .ToList();
            //    b) Store the SelectListItem objects in a SelectList object 
            //       with 'Value' and 'Text' properties set specifically.
            var roleList = new SelectList(preRoleList, "Value", "Text");

            //    c) Store the SelectList in a ViewBag.
            ViewBag.RoleSelectList = roleList;

            // 2. Preparation for 'Users' drop down list. 
            //    a) Build a list of SelectListItem objects which have 'Value' and 
            //       'Text' properties.
            var userList = _context.Users.ToList();

            //    b) Store the SelectListItem objects in a SelectList object 
            //       with 'Value' and 'Text' properties set specifically.
            var preUserList = userList.Select(u =>
                new SelectListItem { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList
                                                        , "Value"
                                                        , "Text");

            //    c) Store the SelectList in a ViewBag.
            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
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
        [ActionName("Delete")]
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

            // Return to the same detail page to show the updated list of roles
            return RedirectToAction("Detail", new { userName });
        }
    }
}
