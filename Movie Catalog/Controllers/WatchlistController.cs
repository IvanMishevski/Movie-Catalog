using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;

namespace Movie_Catalog.Controllers
{
    public class WatchlistController : Controller
    {
        // Core dependencies for watchlist management
        private readonly MovieCatalogContext _context;             // Database context
        private readonly UserManager<ApplicationUser> _userManager; // User management

        // Constructor with dependency injection
        public WatchlistController(
            MovieCatalogContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays the user's watchlist
        public IActionResult Index()
        {
            // Get current user's ID
            var userId = _userManager.GetUserId(User);

            // Retrieve watchlist items with related movie data
            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Genre)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Director)
                .ToList();
            return View(watchlistItems);
        }

        // Adds a movie to user's watchlist
        public async Task<IActionResult> AddToWatchlist(int movieId)
        {
            var userId = _userManager.GetUserId(User);

            // Create new watchlist entry
            var watchlistItem = new Watchlist
            {
                MovieId = movieId,
                UserId = userId,
            };

            // Save to database
            _context.Watchlists.Add(watchlistItem);
            await _context.SaveChangesAsync();

            // Refresh watchlist data for view
            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Genre)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Director)
                .ToList();

            return View("Index", watchlistItems);
        }

        // Removes a movie from user's watchlist
        public async Task<IActionResult> RemoveFromWatchlist(int movieId)
        {
            var userId = _userManager.GetUserId(User);

            // Find and remove watchlist item
            var watchlistItem = _context.Watchlists.FirstOrDefault(w => w.MovieId == movieId && w.UserId == userId);
            if (watchlistItem != null)
            {
                _context.Watchlists.Remove(watchlistItem);
            }
            await _context.SaveChangesAsync();

            // Refresh watchlist data for view
            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Genre)
                .Include(m => m.Movie)
                .ThenInclude(m => m.Director)
                .ToList();
            return View("Index", watchlistItems);
        }
    }
}
