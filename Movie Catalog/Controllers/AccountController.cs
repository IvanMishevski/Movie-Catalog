using Microsoft.AspNetCore.Mvc;

namespace Movie_Catalog.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
