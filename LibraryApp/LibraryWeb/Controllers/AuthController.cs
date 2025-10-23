using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class AuthController : Controller
    {
        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("loggedIn", "true");
                return RedirectToAction("Index", "Home"); // <-- редирект на главную
            }

            ViewData["Error"] = "Неверное имя пользователя или пароль";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedIn");
            return RedirectToAction("Login");
        }
    }
}
