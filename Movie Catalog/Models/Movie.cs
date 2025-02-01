using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
