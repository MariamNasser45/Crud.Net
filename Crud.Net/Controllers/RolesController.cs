using Crud.Net.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud.Net.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class RolesController : Controller
    {
        //define  rolemanager

        private readonly RoleManager<IdentityRole> _roleManager;
  

        // to use previus definations must inject (pass) them in constructor

        //creat constructor

        public RolesController( RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //define action of RolesIndex
        public async Task<IActionResult> RolesIndex()
        {
            // define variable 'roles' to return list of all roles in db
            // define which appear from returne view(roles)

            var roles = await _roleManager.Roles.ToListAsync();           

            return View(roles);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Add(RoleFormViewModel addrole)
        {
            // cheack view model to ensure there are no problem in it
            if (!ModelState.IsValid)
                return View("RolesIndex", await _roleManager.Roles.ToListAsync());

            // cheack if exist role in DB with the same name of add new role to avoid dublicate data error

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
             
                ModelState.AddModelError("Name", "Role is exists!"); // error massage in case of exist role
                return View("RolesIndex", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim())); //.Trim : to remove unuseful spaces 

            return RedirectToAction(nameof(RolesIndex));
        }
    }
    }

