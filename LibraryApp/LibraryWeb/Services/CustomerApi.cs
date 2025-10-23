using System.Net.Http.Json;
using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class CustomerApi
    {
        private readonly HttpClient _http;

        public CustomerApi(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            return await _http.GetFromJsonAsync<List<CustomerDto>>("api/customers");
        }

        public async Task UpdateCustomerAsync(CustomerDto customer)
        {
            await _http.PutAsJsonAsync($"api/customers/{customer.Customerid}", customer);
        }

        public async Task AddCustomerAsync(CustomerDto customer)
        {
            await _http.PostAsJsonAsync("api/customers", customer);
        }
    }
}
