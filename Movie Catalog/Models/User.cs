using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Catalog.Models
{
    public class User
    {
        public int Id { get; set; }


        [MaxLength(50)]
        [Required]
        public string? Username { get; set; }

        [MaxLength(100)]
        [Required]
        public string? Email { get; set; }

        [MaxLength(225)]
        [Required]
        public string? Password { get; set; }

        [Required]
        public Role Role { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum Role
    {
        Admin,
        User
    }
}
