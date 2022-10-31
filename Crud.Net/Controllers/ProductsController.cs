using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Crud.Net.Constant;
using Microsoft.AspNetCore.Identity;

namespace Crud.Net.Controllers
{

    public class ProductsController : Controller
    {
        public IActionResult ProductsIndex()
        {
            return View();
        }

        //Create Edit Action

        [Authorize(Permisions.Products.Edit)] //making onle roles have this permission can work with edit
        public IActionResult Edit()
        {
            return View();
        }

    }
}
