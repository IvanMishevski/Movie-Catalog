using System.ComponentModel.DataAnnotations;

namespace Movie_Catalog.Models
{
    public class RegisterViewModelBase
    {

        [Required]
        [DataType(DataType.Text)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public required string Password { get; set; }
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }
    }
}
