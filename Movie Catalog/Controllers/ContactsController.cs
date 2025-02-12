using Microsoft.AspNetCore.Mvc;

namespace Movie_Catalog.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View("Contacts");
        }
    }
}
