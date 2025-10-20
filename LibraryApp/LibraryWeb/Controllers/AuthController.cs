using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
