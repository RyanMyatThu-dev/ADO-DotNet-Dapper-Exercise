# Database Schema Specification — Book Management

> **Date:** June 7, 2026  
> **Project:** Book_Management (.NET 8, PostgreSQL)  
> **Scope:** Single-table schema for a Book Management API with ADO.NET, Dapper, and EF Core implementations

---

## 1. Database & Connection

- **Database System:** PostgreSQL 17 (Alpine)
- **Database Name:** `Books`
- **Hosting:** docker-compose.yml (`postgres:17-alpine`) + standalone Dockerfile
- **Connection String:** `Host=localhost;Port=5432;Database=Books;Username=postgres;Password=postgres`
- **NuGet Package for ADO.NET/Dapper:** `Npgsql` (switch from `Microsoft.Data.SqlClient`)
- **EF Core Provider:** `Npgsql.EntityFrameworkCore.PostgreSQL`

---

## 2. Tables

### 2.1 `Books`

The only table in the schema. Represents a book in a personal library.

| Column          | Type              | Constraints                        | Notes                               |
|-----------------|-------------------|------------------------------------|-------------------------------------|
| `Id`            | `SERIAL` / `INT`  | `PRIMARY KEY`, auto-increment      |                                     |
| `Title`         | `VARCHAR(200)`    | `NOT NULL`                         |                                     |
| `Author`        | `VARCHAR(100)`    | `NOT NULL`                         |                                     |
| `Genre`         | `VARCHAR(50)`     | `NULL`                             | Plain string, e.g. "Fiction"        |
| `Description`   | `VARCHAR(1000)`   | `NULL`                             |                                     |
| `PublishedDate` | `TIMESTAMP`       | `NULL`                             | Optional — can be unknown           |
| `IsDeleted`     | `BOOLEAN`         | `NOT NULL`, `DEFAULT FALSE`        | Soft-delete flag                    |
| `CreatedAt`     | `TIMESTAMP`       | `NOT NULL`, `DEFAULT NOW()`        | Audit column                        |
| `UpdatedAt`     | `TIMESTAMP`       | `NOT NULL`, `DEFAULT NOW()`        | Audit column                        |

### 2.2 Indexes

| Index Name             | Column(s)  | Type       | Purpose                            |
|------------------------|------------|------------|------------------------------------|
| `IX_Books_Title`       | `Title`    | `BTREE`    | Faster lookups/search by title     |
| `IX_Books_Author`      | `Author`   | `BTREE`    | Faster lookups/search by author    |

No unique constraints (ISBN was removed from the schema).

---

## 3. Entity Model (`Book.cs`)

```csharp
namespace Book_Management.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Changes from current entity:**
- ❌ Removed `ISBN`
- ✅ Added `Genre` (nullable `string`)
- ✅ Added `Description` (nullable `string`)
- ✅ Added `IsDeleted` (boolean, default false)
- ✅ Added `CreatedAt` (DateTime)
- ✅ Added `UpdatedAt` (DateTime)

---

## 4. Repository Interface (`IBookRepository`)

The existing interface is **unchanged** in method signatures:

```csharp
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<int> CreateAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
}
```

**Behavioral changes:**
- `GetAllAsync()` and `GetByIdAsync()` must **filter out soft-deleted** books (`WHERE IsDeleted = FALSE`)
- `DeleteAsync()` should perform a **soft delete** (`UPDATE SET IsDeleted = TRUE, UpdatedAt = NOW()`) instead of a hard `DELETE`
- `UpdateAsync()` should also update `UpdatedAt` to the current timestamp

---

## 5. SQL Migrations / Schema DDL

```sql
CREATE TABLE Books (
    Id          SERIAL        PRIMARY KEY,
    Title       VARCHAR(200)  NOT NULL,
    Author      VARCHAR(100)  NOT NULL,
    Genre       VARCHAR(50)   NULL,
    Description VARCHAR(1000) NULL,
    PublishedDate TIMESTAMP   NULL,
    IsDeleted   BOOLEAN       NOT NULL DEFAULT FALSE,
    CreatedAt   TIMESTAMP     NOT NULL DEFAULT NOW(),
    UpdatedAt   TIMESTAMP     NOT NULL DEFAULT NOW()
);

CREATE INDEX IX_Books_Title  ON Books (Title);
CREATE INDEX IX_Books_Author ON Books (Author);
```

---

## 6. SQL Queries for Repositories

### 6.1 ADO.NET (using `Npgsql`)

| Method         | SQL Command                                                                                     |
|----------------|-------------------------------------------------------------------------------------------------|
| `GetAllAsync`  | `SELECT Id, Title, Author, Genre, Description, PublishedDate, IsDeleted, CreatedAt, UpdatedAt FROM Books WHERE IsDeleted = FALSE` |
| `GetByIdAsync` | Same as above + `WHERE Id = @Id AND IsDeleted = FALSE`                                          |
| `CreateAsync`  | `INSERT INTO Books (Title, Author, Genre, Description, PublishedDate) VALUES (@Title, @Author, @Genre, @Description, @PublishedDate) RETURNING Id` |
| `UpdateAsync`  | `UPDATE Books SET Title=@Title, Author=@Author, Genre=@Genre, Description=@Description, PublishedDate=@PublishedDate, UpdatedAt=NOW() WHERE Id=@Id AND IsDeleted=FALSE` |
| `DeleteAsync`  | `UPDATE Books SET IsDeleted = TRUE, UpdatedAt = NOW() WHERE Id = @Id`                           |

### 6.2 Dapper (using `Npgsql`)

Same SQL as ADO.NET above, executed via Dapper extension methods (`QueryAsync`, `QueryFirstOrDefaultAsync`, `QuerySingleAsync`, `ExecuteAsync`).

### 6.3 EF Core (`AppDbContext`)

- `DbSet<Book> Books` property on `AppDbContext`
- Soft-delete handled via a **query filter**: `.HasQueryFilter(b => !b.IsDeleted)`
- `CreatedAt` / `UpdatedAt` set via overriding `SaveChangesAsync()`

---

## 7. Seed Data

Approximately **30 sample books** covering a variety of genres (Fiction, Non-Fiction, Science, History, Fantasy, etc.) to be inserted as part of initial schema setup.

**Seed strategy:** Insert via raw SQL or EF Core model builder's `HasData()` method.

---

## 8. EF Core `AppDbContext` Design

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).HasMaxLength(200).IsRequired();
            entity.Property(b => b.Author).HasMaxLength(100).IsRequired();
            entity.Property(b => b.Genre).HasMaxLength(50);
            entity.Property(b => b.Description).HasMaxLength(1000);
            entity.Property(b => b.IsDeleted).HasDefaultValue(false);
            entity.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(b => b.UpdatedAt).HasDefaultValueSql("NOW()");

            entity.HasIndex(b => b.Title).HasDatabaseName("IX_Books_Title");
            entity.HasIndex(b => b.Author).HasDatabaseName("IX_Books_Author");

            entity.HasQueryFilter(b => !b.IsDeleted);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-set CreatedAt/UpdatedAt
        foreach (var entry in ChangeTracker.Entries<Book>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
```

---

## 9. Environment & Configuration

- **Docker:** docker-compose.yml runs PostgreSQL 17 Alpine (already configured)
- **Dockerfile:** Standalone Postgres image kept alongside compose file
- **appsettings.json:** Connection string stays the same
- **Program.cs:** Will need ADO.NET/Dapper repos switched to use `Npgsql` instead of `Microsoft.Data.SqlClient`

---

## 10. Implementation Order (Recommended)

1. Update `Book.cs` entity to match the new schema
2. Install `Npgsql` NuGet package in ADO.NET and Dapper projects (remove `Microsoft.Data.SqlClient`)
3. Create/update the `AppDbContext` with `DbSet<Book>`, query filters, and audit auto-setup
4. Write/execute the DDL migration script to create the `Books` table
5. Seed ~30 sample books
6. Implement ADO.NET repository methods (user's manual practice)
7. Implement Dapper repository methods (user's manual practice)
8. Ensure `Program.cs` DI wiring uses the correct Npgsql-based repositories
9. Test all CRUD endpoints via Swagger
