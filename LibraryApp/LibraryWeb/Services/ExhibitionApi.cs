using LibraryWeb.Models;
using System.Net.Http.Json;

public class ExhibitionsApi
{
    private readonly HttpClient _http;
    public ExhibitionsApi(HttpClient http) => _http = http;

    public async Task<List<ExhibitionDto>> GetAllAsync()
    {
        try
        {
            var data = await _http.GetFromJsonAsync<List<ExhibitionDto>>("api/exhibitions");
            return data ?? new List<ExhibitionDto>();
        }
        catch
        {
            return new List<ExhibitionDto>();
        }
    }

    public async Task<ExhibitionDto?> GetByIdAsync(int id)
    {
        if (id <= 0) return null;
        try
        {
            return await _http.GetFromJsonAsync<ExhibitionDto>($"api/exhibitions/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(CreateExhibitionDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/exhibitions", dto);
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateExhibitionDto dto)
    {
        try
        {
            var res = await _http.PutAsJsonAsync($"api/exhibitions/{id}", dto);
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

