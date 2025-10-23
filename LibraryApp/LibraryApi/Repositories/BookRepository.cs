using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Repositories
{
    public interface IBookRepository
    {
        Task<List<Books>> GetAllAsync();
        Task<Books?> GetByKeyAsync(string key);
        Task<List<Books>> SearchAsync(string? title, string? author);
        Task<Books?> GetByKeyWithRelationsAsync(string key);
        Task AddAsync(Books book);
        Task UpdateAsync(Books book);
        Task DeleteAsync(Books book);
    }

    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context) => _context = context;

        public async Task<List<Books>> GetAllAsync() =>
            await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .ToListAsync();

        public async Task<Books?> GetByKeyAsync(string key) =>
            await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .FirstOrDefaultAsync(b => b.Key == key);

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

        public async Task<Books?> GetByKeyWithRelationsAsync(string key) =>
            await _context.Books
                .Include(b => b.Bookcovers)
                .Include(b => b.Booksubjects)
                .Include(b => b.Authorkey)
                .FirstOrDefaultAsync(b => b.Key == key);

        public async Task AddAsync(Books book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Books book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Books book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
