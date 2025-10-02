using Microsoft.AspNetCore.Mvc;
using LibraryWeb.Services;
using LibraryWeb.Models;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookService _bookService;

        public HomeController(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooksAsync();
            return View(books);
        }
    }
}
