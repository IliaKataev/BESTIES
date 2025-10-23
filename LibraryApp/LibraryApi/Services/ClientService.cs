// Интерфейс
using LibraryApi.Models;
using LibraryApi.Repositories;

public interface ICustomerService
{
    Task<Customers?> LoginAsync(string phone);
    Task<Customers?> GetCustomerByIdAsync(long id);
    Task<List<Customers>> GetAllCustomersAsync();
    Task AddCustomerAsync(Customers customer);
    Task UpdateCustomerAsync(Customers customer);


}

// Реализация
public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customers> _customerRepo;

    public CustomerService(IGenericRepository<Customers> customerRepo)
    {
        _customerRepo = customerRepo;
    }

    public async Task<Customers?> LoginAsync(string phone)
    {
        var customers = await _customerRepo.FindAsync(c => c.Phone == phone);
        return customers.FirstOrDefault();
    }

    public async Task<List<Customers>> GetAllCustomersAsync() =>
        await _customerRepo.GetAllAsync();

    public async Task AddCustomerAsync(Customers customer) =>
    await _customerRepo.AddAsync(customer);

    public async Task UpdateCustomerAsync(Customers customer) =>
        await _customerRepo.UpdateAsync(customer);


    public async Task<Customers?> GetCustomerByIdAsync(long id) => await _customerRepo.GetByIdAsync(id);
}
