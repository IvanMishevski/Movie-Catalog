using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Catalog.Models
{
    public class Review
    {
        public int Id { get; set; } 

        [Required]
        public string UserId { get; set; } 
        public ApplicationUser? User { get; set; }

        [Required]
        public int MovieId { get; set; } 
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; } 

        public string? ReviewText { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
