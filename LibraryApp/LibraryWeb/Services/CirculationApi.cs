using System.Net.Http.Json;
using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class CirculationApi
    {
        private readonly HttpClient _http;
        public CirculationApi(HttpClient http) => _http = http;

        // --- Новый метод: получаем сразу всё о клиенте ---
        public async Task<CustomerCirculationDto?> GetCustomerCirculationAsync(long customerId)
        {
            try
            {
                return await _http.GetFromJsonAsync<CustomerCirculationDto>($"api/issues/customer/{customerId}");
            }
            catch
            {
                return null;
            }
        }

        // --- Для совместимости: получаем активные книги клиента ---
        public async Task<List<IssueDto>> GetCurrentIssuesAsync(string customerId)
        {
            try
            {
                var data = await GetCustomerCirculationAsync(long.Parse(customerId));
                return data?.CurrentIssues ?? new List<IssueDto>();
            }
            catch
            {
                return new List<IssueDto>();
            }
        }

        // --- Для совместимости: получаем историю ---
        public async Task<List<IssueDto>> GetHistoryAsync(string customerId)
        {
            try
            {
                var data = await GetCustomerCirculationAsync(long.Parse(customerId));
                return data?.History ?? new List<IssueDto>();
            }
            catch
            {
                return new List<IssueDto>();
            }
        }

        // --- Выдача книги ---
        public async Task<bool> IssueBookAsync(string customerId, string bookKey)
        {
            try
            {
                var res = await _http.PostAsync($"api/issue/{customerId}/{bookKey}", JsonContent.Create(new { }));
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // --- Возврат книги ---
        public async Task<bool> ReturnBookAsync(string customerId, string bookKey)
        {
            try
            {
                var res = await _http.PostAsync($"api/issues/return/{customerId}/{bookKey}", JsonContent.Create(new { }));
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // --- Продление книги ---
        public async Task<bool> RenewBookAsync(long issueId)
        {
            try
            {
                var res = await _http.PostAsync($"api/issues/renew/{issueId}", JsonContent.Create(new { }));
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }

    // DTO для данных о клиенте
    public class CustomerCirculationDto
    {
        public CustomerDto? Customer { get; set; }
        public List<IssueDto> CurrentIssues { get; set; } = new();
        public List<IssueDto> History { get; set; } = new();
    }
}
