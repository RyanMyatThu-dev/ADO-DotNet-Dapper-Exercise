using Book_Management.Domain.Repositories;
using AdoNetRepo = Book_Management.Domain.AdoNet.Repositories.BookRepository;
using DapperRepo = Book_Management.Domain.Dapper.Repositories.BookRepository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register ADO.NET implementation
builder.Services.AddSingleton<IBookRepository>(_ => new AdoNetRepo(connectionString));

// Register Dapper implementation (commented out — uncomment to use Dapper instead)
// builder.Services.AddSingleton<IBookRepository>(_ => new DapperRepo(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
