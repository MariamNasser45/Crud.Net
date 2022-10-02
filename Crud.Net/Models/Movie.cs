namespace Crud.Net.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string ? Name { get; set; }
        public string? History { get; set; }
        public float Rate { get; set; }

        // define relationship between tables
        public int? GenreId { get; set; }

        public Genre Genre { get; set; }

    }
}
