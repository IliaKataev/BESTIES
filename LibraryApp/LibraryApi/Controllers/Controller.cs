using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .ToListAsync();

            var booksDto = books.Select(b => MapToDto(b)).ToList();

            return Ok(booksDto);
        }

        // GET: api/Books/{key}
        [HttpGet("{key}")]
        public async Task<ActionResult<BookDto>> GetBook(string key)
        {
            var book = await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .FirstOrDefaultAsync(b => b.Key == key);

            if (book == null)
                return NotFound();

            return Ok(MapToDto(book));
        }

        // --- Private mapping method ---
        private static BookDto MapToDto(Books b)
        {
            return new BookDto
            {
                Key = b.Key,
                Title = b.Title,
                Subtitle = b.Subtitle,
                FirstPublishDate = b.Firstpublishdate,
                Description = b.Description,
                Covers = b.Bookcovers.Select(c => new BookCoverDto
                {
                    Id = c.Id,
                    CoverFile = c.Coverfile
                }).ToList(),
                Subjects = b.Booksubjects.Select(s => new BookSubjectDto
                {
                    Id = s.Id,
                    Subject = s.Subject
                }).ToList(),
                Authors = b.Authorkey.Select(a => new AuthorDto
                {
                    Key = a.Key,
                    Name = a.Name,
                    Bio = a.Bio,
                    BirthDate = a.Birthdate,
                    DeathDate = a.Deathdate,
                    Wikipedia = a.Wikipedia
                }).ToList()
            };
        }
    }
}
