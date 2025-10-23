using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class CirculationController : Controller
    {
        private readonly CirculationApi _api;

        public CirculationController(CirculationApi api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            // Начало с пустыми списками
            ViewBag.CurrentIssues = new List<IssueDto>();
            ViewBag.History = new List<IssueDto>();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadCustomer(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return Json(new { success = false, message = "Customer ID is required" });

            var current = await _api.GetCurrentIssuesAsync(customerId);
            var history = await _api.GetHistoryAsync(customerId);

            if (current.Count == 0 && history.Count == 0)
                return Json(new { success = false, message = "Customer not found" });

            return Json(new { success = true, currentIssues = current, history });
        }

        [HttpPost]
        public async Task<IActionResult> IssueBook(string customerId, string bookKey)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(bookKey))
                return Json(new { success = false, message = "Customer ID and Book Key are required" });

            var result = await _api.IssueBookAsync(customerId, bookKey);
            if (!result)
                return Json(new { success = false, message = "Cannot issue book (limit reached or invalid book)" });

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(string customerId, string bookKey)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(bookKey))
                return Json(new { success = false, message = "Customer ID and Book Key are required" });

            var result = await _api.ReturnBookAsync(customerId, bookKey);
            if (!result)
                return Json(new { success = false, message = "Cannot return book" });

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RenewBook(long issueId)
        {
            var result = await _api.RenewBookAsync(issueId);
            if (!result)
                return Json(new { success = false, message = "Cannot renew book" });

            return Json(new { success = true });
        }
    }
}
