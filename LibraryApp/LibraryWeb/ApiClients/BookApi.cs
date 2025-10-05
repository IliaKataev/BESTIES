using System.Net.Http.Json;
using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class BookApi
    {
        private readonly HttpClient _http;

        public BookApi(HttpClient http)
        {
            _http = http;
        }

        // Получение списка книг
        public async Task<List<BookDto>> GetBooksAsync()
        {
            return await _http.GetFromJsonAsync<List<BookDto>>("api/books");
        }

        // Получение книги по ключу
        public async Task<BookDto?> GetBookByKeyAsync(string key)
        {
            return await _http.GetFromJsonAsync<BookDto>($"api/books/{key}");
        }
    }
}
