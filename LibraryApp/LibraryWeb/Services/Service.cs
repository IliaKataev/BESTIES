using System.Net.Http.Json;
using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class BookService
    {
        private readonly HttpClient _http;

        public BookService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<BookDto>> GetBooksAsync()
        {
            return await _http.GetFromJsonAsync<List<BookDto>>("api/books");
        }
    }
}
