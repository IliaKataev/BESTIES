using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class BookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        // возвращаем все книги с авторами, обложками и subjects
        public async Task<List<Books>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .ToListAsync();
        }

        // возвращаем конкретную книгу по ключу
        public async Task<Books?> GetByKeyAsync(string key)
        {
            return await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .FirstOrDefaultAsync(b => b.Key == key);
        }

        // ваоиск по названию и автору
        public async Task<List<Books>> SearchAsync(string? title, string? author)
        {
            var query = _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrEmpty(author))
                query = query.Where(b => b.Authorkey.Any(a => a.Name.Contains(author)));

            return await query.ToListAsync();
        }
    }
}
