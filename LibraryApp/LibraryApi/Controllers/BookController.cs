using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<BookDto>> GetByKey(string key)
        {
            var book = await _bookService.GetBookDetailsAsync(key);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<BookDto>>> Search([FromQuery] string? title, [FromQuery] string? author)
        {
            var books = await _bookService.SearchBooksAsync(title, author);
            return Ok(books);
        }

        [HttpGet("info/{keyOrTitle}")]
        public async Task<ActionResult<BookInfoDto>> GetBookInfo(string keyOrTitle)
        {
            if (string.IsNullOrWhiteSpace(keyOrTitle))
                return BadRequest("Key or Title is required");

            // Сначала ищем по ключу
            var book = await _bookService.GetBookDetailsAsync(keyOrTitle);

            // Если не нашли по ключу — ищем по названию
            if (book == null)
            {
                var allBooks = await _bookService.SearchBooksAsync(keyOrTitle, null);
                book = allBooks.FirstOrDefault();
            }

            if (book == null)
                return NotFound("Book not found");

            return Ok(new BookInfoDto
            {
                Title = book.Title,
                Subtitle = book.Subtitle
            });
        }

        public class BookInfoDto
        {
            public string Title { get; set; } = string.Empty;
            public string? Subtitle { get; set; }
        }

    }
}
