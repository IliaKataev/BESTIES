using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Authors>> GetAllAsync();
        Task<Authors?> GetByKeyAsync(string key);
        Task<List<Authors>> SearchAsync(string name);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;
        public AuthorRepository(LibraryContext context) => _context = context;

        public async Task<List<Authors>> GetAllAsync() => await _context.Authors.ToListAsync();

        public async Task<Authors?> GetByKeyAsync(string key) =>
            await _context.Authors.FirstOrDefaultAsync(a => a.Key == key);

        public async Task<List<Authors>> SearchAsync(string name) =>
            await _context.Authors.Where(a => a.Name.Contains(name)).ToListAsync();
    }
}
