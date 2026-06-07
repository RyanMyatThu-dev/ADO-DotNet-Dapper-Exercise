using Dapper;
using Npgsql;
using Book_Management.Domain.Models;
using Book_Management.Domain.Services;
using System.Data;

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
        using var connection = new NpgsqlConnection(_connectionString);
        string query = "SELECT id, title, author, genre, description, publisheddate FROM books WHERE isdeleted = FALSE";
        return await connection.QueryAsync<BookModel>(query);
    }

    public async Task<BookModel?> GetByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string query = "SELECT id, title, author, genre, description, publisheddate FROM books WHERE id = @Id AND isdeleted = FALSE";
        return await connection.QueryFirstOrDefaultAsync<BookModel>(query, new { Id = id });
    }

    public async Task<int> CreateAsync(BookModel book)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string query = "INSERT INTO books (title, author, genre, description, publisheddate) VALUES (@Title, @Author, @Genre, @Description, @PublishedDate) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(query, book);
    }

    public async Task<bool> UpdateAsync(BookModel book)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string query = "UPDATE books SET title = @Title, author = @Author, genre = @Genre, description = @Description, publisheddate = @PublishedDate WHERE id = @Id AND isdeleted = FALSE";
        int rowsAffected = await connection.ExecuteAsync(query, book);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        string query = "UPDATE books SET isdeleted = TRUE WHERE id = @Id";
        int rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }
}
