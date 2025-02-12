using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;

namespace Movie_Catalog.Controllers
{
    public class StatisticsAndReviewsController : Controller
    {
        private readonly ILogger<StatisticsAndReviewsController> _logger;
        private readonly MovieCatalogContext _context;

        public StatisticsAndReviewsController(ILogger<StatisticsAndReviewsController> logger, MovieCatalogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
            .Include(m=>m.Statistic)
            .Include(m=>m.Reviews)
            .AsQueryable();

            return View("StatisticsAndReviews",movies.ToList());
        }
    }
}
