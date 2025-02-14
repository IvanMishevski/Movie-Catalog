using Microsoft.AspNetCore.Identity;

namespace Movie_Catalog.Models

{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Password { get; set; }
    }
}
