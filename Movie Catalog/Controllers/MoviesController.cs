using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Diagnostics;

namespace Movie_Catalog.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MovieCatalogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(ILogger<MoviesController> logger, MovieCatalogContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
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
            var userRole = _userManager.GetUserAsync(User).Result?.UserRole;
            ViewData["UserRole"] = userRole;
            return View(movies.ToList());
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Where(m => m.Id == id).FirstOrDefault();
            return View(movie);

        }
        [HttpPost]
        public IActionResult Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            var existingMovie = _context.Movies.Find(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;

            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Where(m => m.Id == id).FirstOrDefault();
            return View(movie);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                return RedirectToAction("Index", "Movies");
            }
            return NotFound();
        }

        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateMovie(Movie movie, int[] selectedActors)
        {
            ModelState.Remove("Genre");
            ModelState.Remove("Director");

            if (ModelState.IsValid)
            {
                var genre = _context.Genres.Find(movie.GenreId);
                var director = _context.Directors.Find(movie.DirectorId);

                if (genre != null && director != null)
                {
                    movie.Genre = genre;
                    movie.Director = director;

                    if (selectedActors != null && selectedActors.Length > 0)
                    {
                        foreach (var actorId in selectedActors)
                        {
                            movie.MovieActors.Add(new MovieActor
                            {
                                MovieId = movie.Id,
                                ActorId = actorId
                            });
                        }
                    }

                    _context.Movies.Add(movie);
                    _context.SaveChanges();

                    var statistics = new Statistic
                    {
                        MovieId = movie.Id,
                        Views = 0,
                        AvgRating = 0
                    };

                    _context.Statistics.Add(statistics);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Movies");
                }
            }

            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            return View(movie);
        }
    }
}
