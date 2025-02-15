using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Diagnostics;

namespace Movie_Catalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieCatalogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
 

        public HomeController(ILogger<HomeController> logger, MovieCatalogContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
            .Include(m => m.Genre)
            .Include(m => m.Director)
            .Include(m=> m.Statistic)
            .OrderByDescending(m => m.Statistic.AvgRating)
            .Take(3)
            .ToList();

            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var movie = _context.Movies
            .Include(m => m.Genre)
            .Include(m => m.Director)
            .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
            .Where(m => m.Id == id)
            .FirstOrDefault();

            var currentUserId = _userManager.GetUserId(User);
            var isInWatchlist = _context.Watchlists
                .Any(w => w.MovieId == id && w.UserId == currentUserId);

            ViewBag.IsInWatchlist = isInWatchlist;

            return View(movie);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
