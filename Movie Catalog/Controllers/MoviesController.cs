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
        // Core dependencies for the controller
        private readonly ILogger<MoviesController> _logger;         // Handles logging
        private readonly MovieCatalogContext _context;             // Database context
        private readonly UserManager<ApplicationUser> _userManager; // User management

        // Constructor with dependency injection
        public MoviesController(ILogger<MoviesController> logger, MovieCatalogContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // Displays movie list with search and filter functionality
        public IActionResult Index(string searchString, string filter)
        {
            // Initialize base query with related data
            var movies = _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .AsQueryable();

            // Apply search filter based on selected criteria
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

            // Apply sorting based on selected filter
            movies = filter switch
            {
                "title" => movies.OrderBy(m => m.Title),
                "genre" => movies.OrderBy(m => m.Genre.Name),
                "year" => movies.OrderBy(m => m.ReleaseYear),
                _ => movies.OrderBy(m => m.Title)
            };

            // Set ViewData for maintaining state and user information
            ViewData["CurrentFilter"] = filter;
            ViewData["SearchString"] = searchString;
            var userRole = _userManager.GetUserAsync(User).Result?.UserRole;
            ViewData["UserRole"] = userRole;
            return View(movies.ToList());
        }

        // Displays edit form for a movie
        public IActionResult Edit(int id)
        {
            // Populate dropdown lists for the form
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            var movie = _context.Movies.Where(m => m.Id == id).FirstOrDefault();
            ViewBag.SelectedActors = movie.MovieActors.Select(ma => ma.ActorId).ToList();
            return View(movie);
        }

        // Processes the movie edit form submission
        [HttpPost]
        public IActionResult Edit(int id, Movie movie, int[] selectedActorsIds)
        {
            if (ModelState.IsValid)
            {
                // Verify movie exists
                if (id != movie.Id)
                {
                    return NotFound();
                }

                // Retrieve existing movie with related data
                var existingMovie = _context.Movies
                    .Include(m => m.MovieActors)
                    .Include(m => m.Statistic)
                    .FirstOrDefault(m => m.Id == id);

                if (existingMovie == null)
                {
                    return NotFound();
                }

                // Update movie properties
                existingMovie.Title = movie.Title;
                existingMovie.Description = movie.Description;
                existingMovie.ReleaseYear = movie.ReleaseYear;
                existingMovie.GenreId = movie.GenreId;
                existingMovie.DirectorId = movie.DirectorId;
                existingMovie.PosterUrl = movie.PosterUrl;

                // Update actor relationships
                existingMovie.MovieActors.Clear();
                if (selectedActorsIds != null && selectedActorsIds.Length > 0)
                {
                    foreach (var actorId in selectedActorsIds)
                    {
                        existingMovie.MovieActors.Add(new MovieActor
                        {
                            MovieId = existingMovie.Id,
                            ActorId = actorId
                        });
                    }
                }

                _context.Update(existingMovie);
                _context.SaveChanges();
                return RedirectToAction("Index", "Movies");
            }

            // If validation fails, repopulate form data
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            return View(movie);
        }

        // Displays delete confirmation page
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Where(m => m.Id == id).FirstOrDefault();
            return View(movie);
        }

        // Processes movie deletion
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

        // Displays create new movie form
        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            return View();
        }

        // Processes new movie creation
        [HttpPost]
        public IActionResult CreateMovie(Movie movie, int[] selectedActorsIds)
        {
            if (ModelState.IsValid)
            {
                var genre = _context.Genres.Find(movie.GenreId);
                var director = _context.Directors.Find(movie.DirectorId);

                if (genre != null && director != null)
                {
                    // Set movie relationships
                    movie.Genre = genre;
                    movie.Director = director;

                    // Add selected actors
                    if (selectedActorsIds != null && selectedActorsIds.Length > 0)
                    {
                        foreach (var actorId in selectedActorsIds)
                        {
                            movie.MovieActors.Add(new MovieActor
                            {
                                MovieId = movie.Id,
                                ActorId = actorId
                            });
                        }
                    }

                    // Save movie and initialize statistics
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

            // If validation fails, repopulate form data
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Directors = new SelectList(_context.Directors, "DirectorId", "Name");
            ViewBag.Actors = new SelectList(_context.Actors, "Id", "Name");
            return View(movie);
        }

        // Handles movie review submission
        [HttpPost]
        public async Task<IActionResult> SubmitReview(int id, string reviewText, decimal rating)
        {
            var movies = await _context.Movies.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Create and save new review
            var review = new Review
            {
                MovieId = id,
                UserId = user.Id,
                ReviewText = reviewText,
                Rating = rating
            };

            _context.Reviews.Add(review);

            // Update movie statistics
            var movieStats = await _context.Statistics
                .FirstOrDefaultAsync(s => s.MovieId == id);

            if (movieStats != null)
            {
                var allReviews = await _context.Reviews
                    .Where(r => r.MovieId == id)
                    .ToListAsync();

                movieStats.AvgRating = (allReviews.Sum(r => r.Rating) + rating) / (allReviews.Count + 1);
            }

            await _context.SaveChangesAsync();
            return View("Index", movies);
        }
    }
}
