using Book_Management.WebApp.Models;

namespace Book_Management.WebApp.Services;

public interface IBookApiService
{
    Task<IEnumerable<BookViewModel>> GetAllAsync(string provider);
    Task<BookViewModel?> GetByIdAsync(string provider, int id);
    Task<int> CreateAsync(string provider, BookViewModel book);
    Task<bool> UpdateAsync(string provider, BookViewModel book);
    Task<bool> DeleteAsync(string provider, int id);
}
