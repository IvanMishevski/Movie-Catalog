using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;

namespace Movie_Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MovieCatalogContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password policy configuration
                options.Password.RequireDigit = true; // At least one digit (0-9)
                options.Password.RequireLowercase = true; // At least one lowercase letter (a-z)
                options.Password.RequireUppercase = true; // At least one uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = false; // At least one non-alphanumeric character (e.g., !@#$%)
                options.Password.RequiredLength = 8; // Minimum length of 8 characters
                options.Password.RequiredUniqueChars = 1; // At least one unique character
            }
                )
                .AddEntityFrameworkStores<MovieCatalogContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
       

      
    }
}
