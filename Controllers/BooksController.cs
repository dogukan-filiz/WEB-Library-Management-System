using Microsoft.AspNetCore.Mvc;

namespace WEB_Library_Management_System.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
