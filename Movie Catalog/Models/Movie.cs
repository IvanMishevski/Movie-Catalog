using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Movie_Catalog.Models
{
    public class Movie
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(255)]
        public string? Title { get; set; } 

        public string? Description { get; set; } 

        [Required]
        public int ReleaseYear { get; set; } 

        [Required]
        public int Duration { get; set; }

        [Required]
        public int GenreId { get; set; } 
        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        [Required]
        public int DirectorId { get; set; } 
        [ForeignKey("DirectorId")]
        public Director? Director { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>(); 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
