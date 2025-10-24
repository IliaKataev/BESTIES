using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportsApi _api;

        public ReportsController(ReportsApi api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadReminders()
        {
            var reminders = await _api.GetRemindersAsync();
            return Json(reminders);
        }

        [HttpGet]
        public async Task<IActionResult> LoadBookHistory(string bookKeyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(bookKeyOrTitle))
                return Json(new { success = false, message = "Book Key or Title is required" });

            // Получаем историю книги
            var history = await _api.GetBookHistoryAsync(bookKeyOrTitle);
            // Получаем отдельную информацию о книге
            var bookInfo = await _api.GetBookInfoAsync(bookKeyOrTitle);

            if (history == null && bookInfo == null)
                return Json(new { success = false, message = "Book not found or no history" });

            var bookDto = new
            {
                Title = bookInfo?.Title ?? history?.FirstOrDefault()?.BookTitle ?? "[Unknown]",
                Subtitle = bookInfo?.Subtitle ?? ""
            };

            return Json(new
            {
                success = true,
                bookInfo = bookDto,
                history = history ?? new List<IssueDto>()
            });
        }
    }
}
