using Scalar.AspNetCore;
using Book_Management.Domain.Services;
using AdoNetSvc = Book_Management.Domain.AdoNet.Services.BookService;
using DapperSvc = Book_Management.Domain.Dapper.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register ADO.NET implementation (default)
builder.Services.AddSingleton<IBookService>(_ => new AdoNetSvc(connectionString));

// Register Dapper implementation (keyed — used by DapperBooksController)
builder.Services.AddKeyedSingleton<IBookService>("dapper", (_, _) => new DapperSvc(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapSwagger("/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
