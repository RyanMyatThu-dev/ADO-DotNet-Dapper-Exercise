using Microsoft.AspNetCore.Mvc;
using Book_Management.WebApp.Models;
using Book_Management.WebApp.Services;

namespace Book_Management.WebApp.Controllers;

public class BooksController : Controller
{
    private readonly IBookApiService _bookApi;

    public BooksController(IBookApiService bookApi)
    {
        _bookApi = bookApi;
    }

    private static readonly string[] ValidProviders = ["ado", "dapper"];

    private bool IsValidProvider(string? provider) =>
        provider is not null && ValidProviders.Contains(provider);

    // GET /{provider}/books
    [HttpGet("{provider}/books")]
    public async Task<IActionResult> Index(string provider)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        var books = await _bookApi.GetAllAsync(provider);
        ViewBag.Provider = provider;
        ViewBag.ProviderLabel = provider == "ado" ? "ADO.NET" : "Dapper";
        return View(books);
    }

    // GET /{provider}/books/create
    [HttpGet("{provider}/books/create")]
    public IActionResult Create(string provider)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        ViewBag.Provider = provider;
        ViewBag.ProviderLabel = provider == "ado" ? "ADO.NET" : "Dapper";
        return View(new BookViewModel());
    }

    // POST /{provider}/books/create
    [HttpPost("{provider}/books/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string provider, BookViewModel book)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
        {
            ViewBag.Provider = provider;
            ViewBag.ProviderLabel = provider == "ado" ? "ADO.NET" : "Dapper";
            return View(book);
        }

        await _bookApi.CreateAsync(provider, book);
        TempData["Success"] = "Book created successfully.";
        return RedirectToAction(nameof(Index), new { provider });
    }

    // GET /{provider}/books/edit/{id}
    [HttpGet("{provider}/books/edit/{id:int}")]
    public async Task<IActionResult> Edit(string provider, int id)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        var book = await _bookApi.GetByIdAsync(provider, id);
        if (book is null)
            return NotFound();

        ViewBag.Provider = provider;
        ViewBag.ProviderLabel = provider == "ado" ? "ADO.NET" : "Dapper";
        return View(book);
    }

    // POST /{provider}/books/edit/{id}
    [HttpPost("{provider}/books/edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string provider, int id, BookViewModel book)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        if (id != book.Id)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
        {
            ViewBag.Provider = provider;
            ViewBag.ProviderLabel = provider == "ado" ? "ADO.NET" : "Dapper";
            return View(book);
        }

        var updated = await _bookApi.UpdateAsync(provider, book);
        if (!updated)
            return NotFound();

        TempData["Success"] = "Book updated successfully.";
        return RedirectToAction(nameof(Index), new { provider });
    }

    // POST /{provider}/books/delete/{id}
    [HttpPost("{provider}/books/delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string provider, int id)
    {
        if (!IsValidProvider(provider))
            return RedirectToAction("Index", "Home");

        var deleted = await _bookApi.DeleteAsync(provider, id);
        if (!deleted)
            return NotFound();

        TempData["Success"] = "Book deleted successfully.";
        return RedirectToAction(nameof(Index), new { provider });
    }
}
