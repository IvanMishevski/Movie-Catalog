using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Catalog.Models
{
    public class Statistic
    {
        public int Id { get; set; } 

        [Required]
        public int MovieId { get; set; } 
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Views { get; set; } = 0; 

        [Required]
        [Range(0, 10)]
        public decimal AvgRating { get; set; } = 0.00m; 
    }
}
