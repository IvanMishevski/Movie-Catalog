using Microsoft.AspNetCore.Identity;

namespace MovieCatalog.Models

{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Password { get; set; }
    }
}
