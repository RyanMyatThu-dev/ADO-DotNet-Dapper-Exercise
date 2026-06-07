using Dapper;
using Npgsql;
using Book_Management.Domain.Models;
using Book_Management.Domain.Services;

namespace Book_Management.Domain.Dapper.Services;

public class BookService : IBookService
{
    private readonly string _connectionString;

    public BookService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<BookModel>> GetAllAsync()
    {
        throw new NotImplementedException("Implement GetAllAsync using Dapper (Npgsql)");
    }

    public async Task<BookModel?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("Implement GetByIdAsync using Dapper (Npgsql)");
    }

    public async Task<int> CreateAsync(BookModel book)
    {
        throw new NotImplementedException("Implement CreateAsync using Dapper (Npgsql)");
    }

    public async Task<bool> UpdateAsync(BookModel book)
    {
        throw new NotImplementedException("Implement UpdateAsync using Dapper (Npgsql)");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException("Implement DeleteAsync using Dapper (Npgsql)");
    }
}
