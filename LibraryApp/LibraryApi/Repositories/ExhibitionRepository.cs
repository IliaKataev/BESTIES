using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Repositories
{
    public interface IExhibitionRepository
    {
        Task<List<Exhibitions>> GetAllAsync();
        Task<Exhibitions?> GetByIdAsync(int id);
        Task AddAsync(Exhibitions exhibition);
        Task UpdateAsync(Exhibitions exhibition);
        Task DeleteAsync(Exhibitions exhibition);
    }
    public class ExhibitionRepository : IExhibitionRepository
    {
        private readonly LibraryContext _context;
        public ExhibitionRepository(LibraryContext context) => _context = context;

        public async Task<Exhibitions?> GetByIdAsync(int id) =>
            await _context.Exhibitions
                          .Include(e => e.ExhibitionBooks)
                              .ThenInclude(eb => eb.Book)
                                  .ThenInclude(b => b.Bookcovers) // добавлено
                          .Include(e => e.CoverBook)
                              .ThenInclude(cb => cb.Bookcovers) // для coverBook
                          .FirstOrDefaultAsync(e => e.ExhibitionId == id);

        public async Task<List<Exhibitions>> GetAllAsync() =>
            await _context.Exhibitions
                          .Include(e => e.ExhibitionBooks)
                              .ThenInclude(eb => eb.Book)
                                  .ThenInclude(b => b.Bookcovers) // добавлено
                          .ToListAsync();


        public async Task AddAsync(Exhibitions exhibition)
        {
            await _context.Exhibitions.AddAsync(exhibition);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exhibitions exhibition)
        {
            _context.Exhibitions.Update(exhibition);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Exhibitions exhibition)
        {
            _context.Exhibitions.Remove(exhibition);
            await _context.SaveChangesAsync();
        }
    }
}
