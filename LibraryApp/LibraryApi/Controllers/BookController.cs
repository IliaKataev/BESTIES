using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync(); // возвращает List<BookDto>
            return Ok(books);
        }

        // GET: api/books/{key}
        [HttpGet("{key}")]
        public async Task<ActionResult<BookDto>> GetBook(string key)
        {
            var book = await _bookService.GetBookDetailsAsync(key);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // GET: api/books/search?title=...&author=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> Search([FromQuery] string? title, [FromQuery] string? author)
        {
            var result = await _bookService.SearchBooksAsync(title, author);
            return Ok(result);
        }
    }
}
