using Microsoft.AspNetCore.Mvc;
using LibraryWeb.Services;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookApi _bookApi;

        public HomeController(BookApi bookApi)
        {
            _bookApi = bookApi;
        }

        public async Task<IActionResult> Index()
        {
            // Проверка авторизации
            var loggedIn = HttpContext.Session.GetString("loggedIn");
            if (loggedIn != "true")
            {
                return RedirectToAction("Login", "Auth");
            }

            var books = await _bookApi.GetBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(string key)
        {
            var book = await _bookApi.GetBookByKeyAsync(key);
            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
