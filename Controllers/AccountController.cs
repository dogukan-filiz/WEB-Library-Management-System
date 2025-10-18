using Microsoft.AspNetCore.Mvc;

namespace WEB_Library_Management_System.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
