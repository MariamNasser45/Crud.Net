using Crud.Net.Data;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Crud.Net.Controllers 
{
   
    public class MoviesController : Controller
    {
        // since we need to work with DB 
        //Then we take an INSTANCE feom DbContext

        private readonly ApplicationDbContext _Context;

        // and makin instructor take instance of Dbcontext As prametere

        public MoviesController(ApplicationDbContext Context)
        {
            _Context = Context; //now we can working with DB using the last instance
        }

        //To make Index.cshtml able to access data from DB
        //using async and await
        public async Task <IActionResult> Index()
        {
            var movies = await _Context.Movies.ToListAsync();

            return View(movies);
        }
    }
}
