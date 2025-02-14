using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class Watchlist
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; } 

        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; } 

        public DateTime AddedAt { get; set; } = DateTime.UtcNow; 
    }
}
