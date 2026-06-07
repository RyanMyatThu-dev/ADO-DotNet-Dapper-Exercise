using Npgsql;
using Book_Management.Domain.Entities;
using Book_Management.Domain.Repositories;

namespace Book_Management.Domain.AdoNet.Repositories;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        throw new NotImplementedException("Implement GetAllAsync using ADO.NET (Npgsql)");
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("Implement GetByIdAsync using ADO.NET (Npgsql)");
    }

    public async Task<int> CreateAsync(Book book)
    {
        throw new NotImplementedException("Implement CreateAsync using ADO.NET (Npgsql)");
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        throw new NotImplementedException("Implement UpdateAsync using ADO.NET (Npgsql)");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException("Implement DeleteAsync using ADO.NET (Npgsql)");
    }
}
