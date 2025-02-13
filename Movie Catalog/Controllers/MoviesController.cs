using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Diagnostics;

namespace Movie_Catalog.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MovieCatalogContext _context;

        public MoviesController(ILogger<MoviesController> logger, MovieCatalogContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index(string searchString, string filter)
        {
            var movies = _context.Movies
        .Include(m => m.Genre)
        .Include(m => m.Director)
        .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = filter switch
                {
                    "title" => movies.Where(m => m.Title.Contains(searchString)),
                    "genre" => movies.Where(m => m.Genre.Name.Contains(searchString)),
                    "year" => movies.Where(m => m.ReleaseYear.ToString().Contains(searchString)),
                    _ => movies.Where(m => m.Title.Contains(searchString))
                };
            }

            movies = filter switch
            {
                "title" => movies.OrderBy(m => m.Title),
                "genre" => movies.OrderBy(m => m.Genre.Name),
                "year" => movies.OrderBy(m => m.ReleaseYear),
                _ => movies.OrderBy(m => m.Title)
            };

            ViewData["CurrentFilter"] = filter;
            ViewData["SearchString"] = searchString;
            return View(movies.ToList());
        }
        
    }
}
