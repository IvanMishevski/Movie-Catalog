using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class Actor
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } 

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>(); 
    }
}
