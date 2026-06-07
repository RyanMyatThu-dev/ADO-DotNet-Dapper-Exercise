using System.Text;
using System.Text.Json;
using Book_Management.WebApp.Models;

namespace Book_Management.WebApp.Services;

public class BookApiService : IBookApiService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BookApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static string GetEndpoint(string provider) =>
        provider == "dapper" ? "api/dapper-books" : "api/books";

    public async Task<IEnumerable<BookViewModel>> GetAllAsync(string provider)
    {
        var response = await _httpClient.GetAsync(GetEndpoint(provider));
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BookViewModel>>(json, JsonOptions) ?? [];
    }

    public async Task<BookViewModel?> GetByIdAsync(string provider, int id)
    {
        var response = await _httpClient.GetAsync($"{GetEndpoint(provider)}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BookViewModel>(json, JsonOptions);
    }

    public async Task<int> CreateAsync(string provider, BookViewModel book)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(book),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(GetEndpoint(provider), content);
        response.EnsureSuccessStatusCode();

        // The response body contains the created book with its new ID
        var json = await response.Content.ReadAsStringAsync();
        var created = JsonSerializer.Deserialize<BookViewModel>(json, JsonOptions);
        return created?.Id ?? 0;
    }

    public async Task<bool> UpdateAsync(string provider, BookViewModel book)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(book),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PutAsync($"{GetEndpoint(provider)}/{book.Id}", content);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;
        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAsync(string provider, int id)
    {
        var response = await _httpClient.DeleteAsync($"{GetEndpoint(provider)}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;
        response.EnsureSuccessStatusCode();
        return true;
    }
}
