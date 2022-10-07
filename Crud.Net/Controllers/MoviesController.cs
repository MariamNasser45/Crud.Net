using Crud.Net.Data;
using Crud.Net.Models;
using Crud.Net.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;
using System.Xml.Linq;

namespace Crud.Net.Controllers

{
    // model is var of type MovieFormViewModel
    // _Context var of type DbContext
    //movie type of  _Context.Movies
    //viewmodel type of MovieFormViewModel

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
        public async Task<IActionResult> Index()
        {
            var movies = await _Context.Movies.ToListAsync();

            return View(movies); // return list of existing movies in DB
        }

        //Implemention of creat page

        // in past we used in return only name of model without view name beacuse
        // name of view is the same name of index (create) but now
        // we chanage name of view from (Create) to (MovieForm)

        public async Task<IActionResult> Create()
        {
            var viewmodel = new MovieFormViewModel

            {
                //We need populate 'apper in view' only values of Genre Exixt in DB
                // There are two ways
                // 1- have view handling Genres and user can add or remove the movies
                //2- MAKING Data Seeding 'when app open view outomaticlly add data in Db'
                // but we add movies maniualy in DB

                Genres = await _Context.Genres.ToListAsync() // need only value which render in drobdownlist

            };

            return View("MovieForm", viewmodel); //return View model which create page working with it
        }


        // code to validate action of type post


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid) // in case if Model state not exsist returne the model
            {

                model.Genres = await _Context.Genres.ToListAsync(); // to solve problem og genres = null

                return View("MovieForm", model);
            }

            // now after check validate of modelstate need to cheack
            // if any file attach the form or not 'user choos poster or not'

            var files = Request.Form.Files;

            if (!files.Any()) // if there is no file applying the next code
            {
                // since the return is error the must validate genres 

                model.Genres = await _Context.Genres.OrderBy(m => m.Name).ToListAsync(); // to solve problem of genres = null

                ModelState.AddModelError("Poster", "Pleas select movie poster"); // AddModelError tazking two pramer (key well send error to it , alert massage)

                return View("MovieForm", model);

            }

            // to cheack which exetintion and the size of file are allow for user

            // 1- check exetinsion

            var poster = files.FirstOrDefault(); // this var to contain choosen file name

            var allowedExetintions = new List<string> { ".jpg", ".png" };

            if (!allowedExetintions.Contains(Path.GetExtension(poster.FileName).ToLower())) // to cheack if the exe of file one of png,jpg or not
            {
                // if exe is not png , jpg applyin next code                                // ToLower used to if name is capital leter conver to small aoutomatic

                model.Genres = await _Context.Genres.OrderBy(m => m.Name).ToListAsync(); // to solve problem of genres = null

                ModelState.AddModelError("Poster", "Only .png , .jpg are allowed "); // AddModelError : taking two pramer (key well send error to it , alert massage)

                return View("MovieForm", model);

            }
            // 2- cheack size

            if (poster.Length > 1048576) // 1048576 byt = 1 megabyt
            {
                model.Genres = await _Context.Genres.OrderBy(m => m.Name).ToListAsync(); // to solve problem of genres = null

                ModelState.AddModelError("Poster", "Poster is large please select other in range 1 MB "); // AddModelError : taking two pramer (key well send error to it , alert massage)

                return View("MovieForm", model);
            }

            // To store data of form in Db

            using var dataStream = new MemoryStream();

            await poster.CopyToAsync(dataStream);

            //mapping reciev value in form to DB with the same type of values in DB 

            //can use packge automapper for this but we mak it muniwal

            var movies = new Movie

            {
                //names are defined in DB = model . names defined of MovieFormViewModel

                Name = model.Name,
                GenreId = model.GenreId,
                year = model.Year,
                History = model.History,
                Rate = model.Rate,
                Poster = dataStream.ToArray(),

            };

            _Context.Movies.Add(movies);  // movies: variable defined >>>> var movies = new Movie

            _Context.SaveChanges(); // to send changes in form to DB

            /*return View(model);*/    // this is mistake and error will occure because it cannot make population to dropdownlist
                                       // becuas the accept model has null genres so it can not select drobdownlist from null


            // now we need after add movie go to index page so using next return

            return RedirectToAction(nameof(Index));

        } // end of method

        // to provide user ability for editing the movies in site

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            // cheack if Id exist in Db or not , (Id is key of table Movies in db)

            var movie = await _Context.Movies.FindAsync(id);
            {
                if (movie == null)
                    return NotFound();

                var viewmodel = new MovieFormViewModel

                {
                    //need all values in  form inorder to when user open movie show all data about this movie

                    Id = movie.Id,
                    Name = movie.Name,
                    GenreId = movie.GenreId,
                    Year = movie.year,
                    History = movie.History,
                    Rate = movie.Rate,
                    Poster = movie.Poster,

                    Genres = await _Context.Genres.ToListAsync()
                };

                return View("MovieForm", viewmodel); // using the view of create becaus edit , create are identicle wir

            }
           
        } // End of method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid) // in case if Model state not exsist returne the model
            {

                model.Genres = await _Context.Genres.ToListAsync(); // to solve problem of genres = null

                return View("MovieForm", model);
            }
            var movie = await _Context.Movies.FindAsync(model.Id);
            {
                // if user choose ID to edit and this ID not exist in DB then return error 

                if (movie == null)
                    return NotFound();
              
                     movie.Id = model.Id;
                     movie.Name=model.Name;
                     movie.GenreId=model.GenreId;
                     movie.year = model.Year;
                     movie.History = model.History;
                     movie.Rate = model.Rate;
                     
                   _Context.SaveChanges();
                    return RedirectToAction(nameof(Index));

            }


        }
    }
}



//[HttpPost]
//[ValidateAntiForgeryToken]

//public async Task<IActionResult> Edit(MovieFormViewModel model)
//{
//    if (!ModelState.IsValid) // in case if Model state not exsist returne the model
//    {

//        model.Genres = await _Context.Genres.ToListAsync(); // to solve problem of genres = null

//        return View("MovieForm", model);
//    }
//    var movie = await _Context.Movies.FindAsync(model.Id);
//    {
//        // if user choose ID to edit and this ID not exist in DB then return error 

//        if (movie == null)
//            return NotFound();

//    }


//}