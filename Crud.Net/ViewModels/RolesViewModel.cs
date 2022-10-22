
// using only to creat check box in roles view

// create new roleview not using UsersRolesViewModel becaus we need using
// list to store all roles in DB With Check boxes to them

namespace Crud.Net.ViewModels
{
    public class RolesViewModel
    {
        public string RoleName { get; set; }

        public bool IsSelected { get; set; }

        // now go to define list of RolesViewModel in user roles viewmodel

    }
}
