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
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Connection opened successfully.");

            string query = "SELECT \"Id\", \"Title\", \"Author\", \"Genre\", \"Description\", \"PublishedDate\" FROM \"Books\" WHERE \"IsDeleted\" = FALSE";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                List<BookModel> books = new List<BookModel>();
                foreach (DataRow row in dataTable.Rows)
                {
                    books.Add(new BookModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Title = row["Title"].ToString() ?? string.Empty,
                        Author = row["Author"].ToString() ?? string.Empty,
                        Genre = row["Genre"].ToString(),
                        Description = row["Description"].ToString() ?? string.Empty,
                        PublishedDate = Convert.ToDateTime(row["PublishedDate"])
                    });
                }
                return books;
            {
                
            }
        }
    }
    }

    public async Task<BookModel?> GetByIdAsync(int id)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Connection opened successfully.");

            string query = "SELECT \"Id\", \"Title\", \"Author\", \"Genre\", \"Description\", \"PublishedDate\" FROM \"Books\" WHERE \"Id\" = @Id AND \"IsDeleted\" = FALSE";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        return new BookModel
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Title = row["Title"].ToString() ?? string.Empty,
                            Author = row["Author"].ToString() ?? string.Empty,
                            Genre = row["Genre"].ToString(),
                            Description = row["Description"].ToString() ?? string.Empty,
                            PublishedDate = Convert.ToDateTime(row["PublishedDate"])
                        };
                    }
                }
            }
            return null;
        }
    }

    public async Task<int> CreateAsync(BookModel book)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Connection opened successfully.");

            string query = "INSERT INTO \"Books\" (\"Title\", \"Author\", \"Genre\", \"Description\", \"PublishedDate\", \"IsDeleted\", \"CreatedAt\", \"UpdatedAt\") VALUES (@Title, @Author, @Genre, @Description, @PublishedDate, FALSE, NOW(), NOW()) RETURNING \"Id\"";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);

                object result = await command.ExecuteNonQueryAsync();
                return Convert.ToInt32(result);
            }
        }
    }

    public async Task<bool> UpdateAsync(BookModel book)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Connection opened successfully.");

            string query = "UPDATE \"Books\" SET \"Title\" = @Title, \"Author\" = @Author, \"Genre\" = @Genre, \"Description\" = @Description, \"PublishedDate\" = @PublishedDate, \"UpdatedAt\" = NOW() WHERE \"Id\" = @Id AND \"IsDeleted\" = FALSE";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", book.Id);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                if(rowsAffected > 0)
                {
                    Console.WriteLine($"Book with ID {book.Id} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"No book found with ID {book.Id} to update.");
                }
                return rowsAffected > 0;
        }
    }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Connection opened successfully.");

            string query = "UPDATE \"Books\" SET \"IsDeleted\" = TRUE, \"UpdatedAt\" = NOW() WHERE \"Id\" = @Id AND \"IsDeleted\" = FALSE";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
    }
}
}