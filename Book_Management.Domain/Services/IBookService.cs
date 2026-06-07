using Book_Management.Domain.Models;

namespace Book_Management.Domain.Services;

public interface IBookService
{
    Task<IEnumerable<BookModel>> GetAllAsync();
    Task<BookModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(BookModel book);
    Task<bool> UpdateAsync(BookModel book);
    Task<bool> DeleteAsync(int id);
}
