using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        [Required]
        public Movie? Movie { get; set; }

        public int GenreId { get; set; }
        [Required]
        public Genre? Genre { get; set; }
    }
}
