using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public interface IIssueRepository
{
    Task<Issues?> GetByIdAsync(long id);
    Task<List<Issues>> GetByCustomerIdAsync(long customerId);
    Task<List<Issues>> GetActiveIssuesAsync();
    Task<List<Issues>> GetAllAsync();
    Task<Issues> AddAsync(Issues issue);
    Task UpdateAsync(Issues issue);
    Task<List<Issues>> GetByCustomerIdWithDetailsAsync(long customerId);

    Task<List<Issues>> GetActiveIssuesWithDetailsAsync();
}

public class IssueRepository : IIssueRepository
{
    private readonly LibraryContext _context;
    public IssueRepository(LibraryContext context) => _context = context;

    public async Task<List<Issues>> GetAllAsync() =>
        await _context.Issues.Include(i => i.BookkeyNavigation).Include(i => i.Customer).ToListAsync();

    public async Task<Issues?> GetByIdAsync(long id) => await _context.Issues.FindAsync(id);

    public async Task<List<Issues>> GetByCustomerIdAsync(long customerId) =>
        await _context.Issues
            .Include(i => i.BookkeyNavigation)
            .Where(i => i.Customerid == customerId)
            .ToListAsync();

    public async Task<List<Issues>> GetByCustomerIdWithDetailsAsync(long customerId)
    {
        return await _context.Issues
            .Include(i => i.BookkeyNavigation)
            .Include(i => i.Customer)
            .Where(i => i.Customerid == customerId)
            .ToListAsync();
    }

    public async Task<List<Issues>> GetActiveIssuesAsync() =>
        await _context.Issues
            .Include(i => i.BookkeyNavigation)
            .Include(i => i.Customer)
            .Where(i => i.Returndate == null)
            .ToListAsync();

    public async Task<List<Issues>> GetActiveIssuesWithDetailsAsync()
    {
        return await _context.Issues
            .Include(i => i.BookkeyNavigation)
            .Include(i => i.Customer)
            .Where(i => i.Returndate == null)
            .ToListAsync();
    }

    public async Task<Issues> AddAsync(Issues issue)
    {
        await _context.Issues.AddAsync(issue);
        await _context.SaveChangesAsync();
        return issue;
    }

    public async Task UpdateAsync(Issues issue)
    {
        _context.Issues.Update(issue);
        await _context.SaveChangesAsync();
    }
}