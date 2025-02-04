using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class Director
    {
        public int DirectorId { get; set; } 

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } 

        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); 
    }
}
