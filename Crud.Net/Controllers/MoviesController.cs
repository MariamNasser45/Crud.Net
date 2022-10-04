using Crud.Net.Data;
using Crud.Net.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace Crud.Net.Controllers 
{
   
    public class MoviesController : Controller
    {
        // since we need to work with DB 
        //Then we take an INSTANCE feom DbContext

        private readonly ApplicationDbContext _Context;

        // and making instructor take instance of Dbcontext As prametere

        public MoviesController(ApplicationDbContext Context)
        {
            _Context = Context; //now we can working with DB using the last instance
        }

        //To make Index.cshtml able to access data from DB
        //using async and await
        public async Task <IActionResult> Index()
        {
            var movies = await _Context.Movies.ToListAsync();

            return View(movies); // return list of existing movies
        }

                      //Implemention of creat page
    
        public async Task<IActionResult> Create()
        {
            var viewmodel = new MovieFormViewModel

            {
                //We need populate 'apper in view' only values of Genre Exixt in DB
                // There are two ways
                // 1- have view handling Genres and user can add or remove the movies
                //2- MAKING Data Seeding 'when app open view outomaticlly add data in Db'
                // but we add movies maniualy in DB

                Genres = await _Context.Genres.ToListAsync()

            };

            return View(viewmodel); //return View model which create page working with it
        }
    }
}
