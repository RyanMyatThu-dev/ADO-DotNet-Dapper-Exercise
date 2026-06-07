using Microsoft.AspNetCore.Mvc;
using Book_Management.Domain.Models;
using Book_Management.Domain.Services;

namespace Book_Management.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Gets all books (not soft-deleted).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookModel>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    /// <summary>
    /// Gets a book by its ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookModel>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
            return NotFound();
        return Ok(book);
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BookModel>> Create(BookModel book)
    {
        var id = await _bookService.CreateAsync(book);
        book.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, book);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, BookModel book)
    {
        if (id != book.Id)
            return BadRequest("ID mismatch");

        var updated = await _bookService.UpdateAsync(book);
        if (!updated)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Soft-deletes a book by its ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
