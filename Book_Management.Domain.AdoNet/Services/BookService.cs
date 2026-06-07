using Npgsql;
using Book_Management.Domain.Models;
using Book_Management.Domain.Services;
using System.Data;

namespace Book_Management.Domain.AdoNet.Services;

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
        await connection.OpenAsync();

        string query = "SELECT id, title, author, genre, description, publisheddate FROM books WHERE isdeleted = FALSE";
        using var command = new NpgsqlCommand(query, connection);
        using var adapter = new NpgsqlDataAdapter(command);
        
        var dataTable = new DataTable();
        adapter.Fill(dataTable);

        var books = new List<BookModel>();
        foreach (DataRow row in dataTable.Rows)
        {
            books.Add(new BookModel
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString() ?? string.Empty,
                Author = row["author"].ToString() ?? string.Empty,
                Genre = row["genre"] != DBNull.Value ? row["genre"].ToString() : null,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                PublishedDate = row["publisheddate"] != DBNull.Value ? Convert.ToDateTime(row["publisheddate"]) : null
            });
        }
        return books;
    }

    public async Task<BookModel?> GetByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT id, title, author, genre, description, publisheddate FROM books WHERE id = @Id AND isdeleted = FALSE";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var adapter = new NpgsqlDataAdapter(command);
        
        var dataTable = new DataTable();
        adapter.Fill(dataTable);

        if (dataTable.Rows.Count > 0)
        {
            DataRow row = dataTable.Rows[0];
            return new BookModel
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString() ?? string.Empty,
                Author = row["author"].ToString() ?? string.Empty,
                Genre = row["genre"] != DBNull.Value ? row["genre"].ToString() : null,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                PublishedDate = row["publisheddate"] != DBNull.Value ? Convert.ToDateTime(row["publisheddate"]) : null
            };
        }
        return null;
    }

    public async Task<int> CreateAsync(BookModel book)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "INSERT INTO books (title, author, genre, description, publisheddate, isdeleted, createdat, updatedat) VALUES (@Title, @Author, @Genre, @Description, @PublishedDate, FALSE, NOW(), NOW()) RETURNING id";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);

        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(BookModel book)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "UPDATE books SET title = @Title, author = @Author, genre = @Genre, description = @Description, publisheddate = @PublishedDate, updatedat = NOW() WHERE id = @Id AND isdeleted = FALSE";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", book.Id);
        command.Parameters.AddWithValue("@Title", book.Title);
        command.Parameters.AddWithValue("@Author", book.Author);
        command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "UPDATE books SET isdeleted = TRUE, updatedat = NOW() WHERE id = @Id AND isdeleted = FALSE";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}