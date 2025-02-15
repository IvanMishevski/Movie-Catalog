// Required imports for Identity, MVC, and database functionality
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Diagnostics;

namespace Movie_Catalog.Controllers
{
    public class HomeController : Controller
    {
        // Dependencies for logging, database access, and user management
        private readonly ILogger<HomeController> _logger;           // Handles logging
        private readonly MovieCatalogContext _context;             // Database context
        private readonly UserManager<ApplicationUser> _userManager; // User management operations

        // Constructor with dependency injection
        public HomeController(ILogger<HomeController> logger, MovieCatalogContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // Displays the home page with top 3 rated movies
        public IActionResult Index()
        {
            // Retrieve top 3 movies ordered by average rating
            // Including related Genre, Director, and Statistics data
            var movies = _context.Movies
            .Include(m => m.Genre)
            .Include(m => m.Director)
            .Include(m => m.Statistic)
            .OrderByDescending(m => m.Statistic.AvgRating)
            .Take(3)
            .ToList();

            return View(movies);
        }

        // Displays detailed information for a specific movie
        public IActionResult Details(int id)
        {
            // Retrieve specific movie with all related data
            var movie = _context.Movies
            .Include(m => m.Genre)
            .Include(m => m.Director)
            .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
            .Where(m => m.Id == id)
            .FirstOrDefault();

            // Check if the movie is in current user's watchlist
            var currentUserId = _userManager.GetUserId(User);
            var isInWatchlist = _context.Watchlists
                .Any(w => w.MovieId == id && w.UserId == currentUserId);

            ViewBag.IsInWatchlist = isInWatchlist;

            return View(movie);
        }

        // Displays the privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Handles error display
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
