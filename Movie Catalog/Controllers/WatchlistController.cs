using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;

namespace Movie_Catalog.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly MovieCatalogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public WatchlistController(
            MovieCatalogContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;

        }
        public  IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Genre)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Director)
            .ToList();
            return View(watchlistItems);
        }
        public async Task<IActionResult> AddToWatchlist(int movieId)
        {
            var userId = _userManager.GetUserId(User);

            var watchlistItem = new Watchlist
            {
                MovieId = movieId,
                UserId = userId,
            };

            _context.Watchlists.Add(watchlistItem);
            await _context.SaveChangesAsync();

            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Genre)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Director)
            .ToList();
            return View("Index", watchlistItems);
        }
        public async Task<IActionResult> RemoveFromWatchlist(int movieId)
        {

            var userId = _userManager.GetUserId(User);
            var watchlistItem = _context.Watchlists.FirstOrDefault(w => w.MovieId == movieId && w.UserId == userId);
            if (watchlistItem != null)
            {

                _context.Watchlists.Remove(watchlistItem);
            }
            await _context.SaveChangesAsync();
            var watchlistItems = _context.Watchlists.Where(w => w.UserId == userId)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Genre)
            .Include(m => m.Movie)
            .ThenInclude(m => m.Director)
            .ToList();
            return View("Index",watchlistItems);
        }
    }
}
