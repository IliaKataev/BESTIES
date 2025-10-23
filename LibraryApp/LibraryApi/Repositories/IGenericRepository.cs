using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly LibraryContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(LibraryContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<TEntity?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();
    }
}
