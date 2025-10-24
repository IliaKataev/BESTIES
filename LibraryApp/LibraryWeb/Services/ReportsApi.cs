using LibraryWeb.Models;
using System.Net.Http.Json;

namespace LibraryWeb.Services
{
    public class ReportsApi
    {
        private readonly HttpClient _http;
        public ReportsApi(HttpClient http) => _http = http;

        // --- Получаем просроченные выдачи ---
        public async Task<List<IssueDto>> GetRemindersAsync()
        {
            try
            {
                var issues = await _http.GetFromJsonAsync<List<IssueDto>>("api/issues/active");
                return issues?.Where(i => i.ReturnUntil < DateTime.Today && i.ReturnDate == null).ToList() ?? new List<IssueDto>();
            }
            catch
            {
                return new List<IssueDto>();
            }
        }

        // --- История книги по ключу или названию ---
        public async Task<List<IssueDto>?> GetBookHistoryAsync(string bookKeyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(bookKeyOrTitle))
                return null;

            try
            {
                var history = await _http.GetFromJsonAsync<List<IssueDto>>($"api/issues/book-history/{Uri.EscapeDataString(bookKeyOrTitle)}");
                return history?.Count > 0 ? history : null;
            }
            catch
            {
                return null;
            }
        }

        // --- Получаем информацию о книге ---
        public async Task<BookInfoDto?> GetBookInfoAsync(string bookKeyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(bookKeyOrTitle))
                return null;

            try
            {
                // Новый эндпоинт BooksController
                var book = await _http.GetFromJsonAsync<BookInfoDto>($"api/books/info/{Uri.EscapeDataString(bookKeyOrTitle)}");
                return book;
            }
            catch
            {
                return null;
            }
        }

        public class BookInfoDto
        {
            public string Title { get; set; } = string.Empty;
            public string? Subtitle { get; set; }
        }

    }
}
