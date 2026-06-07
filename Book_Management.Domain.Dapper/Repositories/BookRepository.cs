using Dapper;
using Npgsql;
using Book_Management.Domain.Entities;
using Book_Management.Domain.Repositories;


namespace Book_Management.Domain.Dapper.Repositories;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        throw new NotImplementedException("Implement GetAllAsync using Dapper (Npgsql)");
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("Implement GetByIdAsync using Dapper (Npgsql)");
    }

    public async Task<int> CreateAsync(Book book)
    {
        throw new NotImplementedException("Implement CreateAsync using Dapper (Npgsql)");
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        throw new NotImplementedException("Implement UpdateAsync using Dapper (Npgsql)");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException("Implement DeleteAsync using Dapper (Npgsql)");
    }
}
