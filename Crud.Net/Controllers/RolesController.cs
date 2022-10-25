﻿using Crud.Net.Constant;
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

        // create this action to add new roles if there is no exist in table 
        public async Task<IActionResult> Add (RoleFormViewModel addrole)
        {
            // cheack view model to ensure there are no problem in it
            if (!ModelState.IsValid)
                return View("RolesIndex", await _roleManager.Roles.ToListAsync());

            // cheack if exist role in DB with the same name of add new role to avoid dublicate data error

            if (await _roleManager.RoleExistsAsync(addrole.Name))
            {
             
                ModelState.AddModelError("Name", "Role is exists!"); // error massage in case of exist role
                return View("RolesIndex", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(addrole.Name.Trim())); //.Trim : to remove unuseful spaces 

            return RedirectToAction(nameof(RolesIndex));
        }

        //action of manage permission button
        // ManagePermissions : determine which permission apply to specific role
        public async Task<IActionResult> ManagePermissions (string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(roleId == null)
                return NotFound();

            // to show all permission for specific role 'choosen to update it'

            // role ''name of choosen role , 

            // using .select to show only column of value from table claims in DB

            var roleclaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList(); // permissions to current asigned role

            // define all permissions which the end user has them
            var allClaims = Permisions.GenerateAllPermisions(); //return list of all permission 

            //convert all  string 'permissions' into check box

            var allPermissions = allClaims.Select(p => new CheackBoxViewModel { RoleName = p }).ToList();   // p is the permission can be assigent to selected role

            // since the variables (allpermissions : contain all 12 permission and roleclaims the permissions already asigned  to role
            // then we need intersect permissions of them in  orderto view of manage permission contain 12 permissions with
            // select check box of permissions return from roleclaims

            foreach (var permission in allPermissions)
            {
                // return true in case  of: current using permission assigend exist in roleclaims "permission assigned now for this rule

                if (roleclaims.Any(c => c == permission.RoleName))
                    permission.IsSelected=true; 
            }

            // initialize view model which return to ManagePermissions

            var viewmodel = new PermissionsFormViewModel
            {
                RoleName = role.Name,
                RoleId = role.Id,
                RoleCalims = allPermissions
            };
             

            return View(viewmodel);    
            
        }

    }
}
