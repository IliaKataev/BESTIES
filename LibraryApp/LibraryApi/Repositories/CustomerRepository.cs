using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public interface ICustomerRepository
{
    Task<Customers?> GetByIdAsync(long id);
    Task<Customers?> GetByPhoneAsync(string phone);
    Task<List<Customers>> GetAllAsync();
    Task AddAsync(Customers customer);
    Task UpdateAsync(Customers customer);
    Task DeleteAsync(Customers customer);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly LibraryContext _context;
    public CustomerRepository(LibraryContext context) => _context = context;

    public async Task<List<Customers>> GetAllAsync() => await _context.Customers.ToListAsync();
    public async Task<Customers?> GetByIdAsync(long id) => await _context.Customers.FindAsync(id);
    public async Task<Customers?> GetByPhoneAsync(string phone) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Phone == phone);

    public async Task AddAsync(Customers customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customers customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customers customer)
    {
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}