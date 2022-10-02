using Microsoft.AspNetCore.Mvc;

namespace Crud.Net.Controllers
{
    public class MoviesControl_er : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
