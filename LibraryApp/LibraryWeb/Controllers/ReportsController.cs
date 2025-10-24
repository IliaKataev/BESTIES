using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

            if (history == null || !history.Any())
                return Json(new { success = false, message = "No history found" });

            return Json(new { success = true, history });
        }


        [HttpGet]
        public async Task<IActionResult> LoadBookInfo(string bookKeyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(bookKeyOrTitle))
                return Json(new { success = false, message = "Book Key or Title is required" });

            // Получаем информацию о книге
            var bookInfo = await _api.GetBookInfoAsync(bookKeyOrTitle);

            if (bookInfo == null)
                return Json(new { success = false, message = "Book not found" });

            return Json(new { success = true, bookInfo });
        }



        // кнопка экспорта
        [HttpGet]
        public async Task<IActionResult> ExportRemindersCsv()
        {
            var reminders = await _api.GetRemindersAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Title,Customer,Date of Issue,Return Until");

            foreach (var i in reminders)
            {
                sb.AppendLine($"\"{i.BookTitle}\",\"{i.CustomerName}\",\"{i.DateOfIssue:yyyy-MM-dd}\",\"{i.ReturnUntil:yyyy-MM-dd}\"");
            }

            var bytes = Encoding.UTF8.GetBytes("\uFEFF" + sb.ToString()); // BOM для Excel
            return File(bytes, "text/csv", "Reminders.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportBookHistoryCsv(string bookKeyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(bookKeyOrTitle))
                return BadRequest("Book Key or Title is required");

            var history = await _api.GetBookHistoryAsync(bookKeyOrTitle);
            if (history == null || !history.Any())
                return NotFound("No history found");

            var sb = new StringBuilder();
            sb.AppendLine("Customer,Date of Issue,Return Date");

            foreach (var i in history)
            {
                sb.AppendLine($"\"{i.CustomerName}\",\"{i.DateOfIssue:yyyy-MM-dd}\",\"{i.ReturnDate:yyyy-MM-dd}\"");
            }

            var bytes = Encoding.UTF8.GetBytes("\uFEFF" + sb.ToString()); // BOM для Excel
            return File(bytes, "text/csv", $"BookHistory_{bookKeyOrTitle}.csv");
        }

    }
}
