using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class Genre
    {
        
        
            public int Id { get; set; } 

            [Required]
            [MaxLength(50)]
            public string? Name { get; set; } 
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        
    }
}
