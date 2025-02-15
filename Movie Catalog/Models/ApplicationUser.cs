using Microsoft.AspNetCore.Identity;

namespace Movie_Catalog.Models

{
    public class ApplicationUser : IdentityUser
    {
        public string UserRole { get; set; } = "PublicUser";


    }
}
