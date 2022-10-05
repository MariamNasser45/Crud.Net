using Crud.Net.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Crud.Net.ViewModels
{
    public class MovieFormViewModel
    {
        //code smilar to code of Movies whith slitly diff

        [Required, StringLength(250)]
        [Display(Name = "Title")]
        public string? Name { get; set; }

        public int Year { get; set; }

        [Required, StringLength(2500)]
        public string History { get; set; }

        [Range (1, 10)] // Detrmine range which can be accepted according project
        public float Rate { get; set; }

        [Display(Name = "Select Poster")]
        public byte[] ?Poster { get; set; }

        [Display (Name ="Genre")] // change name display for user to be friendly
        public int? GenreId { get; set; }

        // since the genreid user can't enter it the we creat list of genre 'Drop down list'

        public IEnumerable<Genre>? Genres { get; set; }
    }
}
