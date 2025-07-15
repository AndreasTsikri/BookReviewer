using AutoMapper;
using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookReviewer.Controllers;

public class BooksController : Controller
{
    readonly ILogger<BooksController> _logger;
    readonly ApplicationDbContext _ctx;
    readonly IMapper _mapper;

    public BooksController(ILogger<BooksController> logger, ApplicationDbContext ctx/*, IMapper mapper*/)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // var model = new BooksViewModel();
        // model.Books = await _ctx.Books.AsNoTracking().ToListAsync();
        var model = await _ctx.Books.AsNoTracking().ToListAsync();
        return View(model);
    }

    /// <summary>
    /// Show form to create a new book
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([Bind("Title, Author, PublishedYear, Genre")] Book book)
    {
        if (!ModelState.IsValid)
            return View(book);
        await _ctx.Books.AddAsync(book);
        await _ctx.SaveChangesAsync();
        return View(book);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _ctx.Books.FindAsync(id);
        if (b != null)
        {
            _ctx.Books.Remove(b);
            await _ctx.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    // GET: Movies/Edit/5
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var b = await _ctx.Books.FindAsync(id);
        if (b == null)
        {
            return NotFound();
        }
        return View(b);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    // POST: Movies/Edit/5
public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PublishedYear")] Book book)
{
    if (id != book.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _ctx.Update(book);
            await _ctx.SaveChangesAsync();
        
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _ctx.Books.AnyAsync(b => b.Id == book.Id);
            if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
        }
        return RedirectToAction(nameof(Index));
    }
    return View(book);
}

}