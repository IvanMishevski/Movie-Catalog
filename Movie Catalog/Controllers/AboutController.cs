using Microsoft.AspNetCore.Mvc;

namespace Movie_Catalog.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View("About");
        }
    }
}
