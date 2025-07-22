using System.Text;
using AutoMapper;
using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookReviewer.Controllers;

public class BooksController : Controller
{
    readonly ILogger<BooksController> _logger;
    readonly ApplicationDbContext _ctx;
    //readonly IMapper _mapper;

    public BooksController(ILogger<BooksController> logger, ApplicationDbContext ctx/*, IMapper mapper*/)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index(string tso, string gso, string aso, string searchStr)
    {
        bool isAsc(string s) => s == "asc" ;
        
        var model = _ctx.Books.AsNoTracking();
        ViewBag.TitleSortParm = tso == "desc"? "asc" : "desc";
        ViewBag.GenreSortParam = gso == "desc" ? "asc" : "desc";
        ViewBag.AuthorSortParm = aso == "desc" ? "asc" : "desc";
        ViewBag.SearchStr = !string.IsNullOrEmpty(searchStr) ? searchStr : "";

        if (tso != null)
            model = isAsc(tso!) ? model.OrderBy(b => b.Title) : model.OrderByDescending(b => b.Title);
        if(gso != null)
            model = isAsc(gso!) ? model.OrderBy(b => b.Genre) : model.OrderByDescending(b => b.Genre);
        if(aso != null)
            model = isAsc(aso!) ? model.OrderBy(b => b.Author) : model.OrderByDescending(b => b.Author);

        if (!string.IsNullOrEmpty(searchStr))
           model = model.Where(b => b.Title.Contains(searchStr));
        // var model = new BooksViewModel();
            // model.Books = await _ctx.Books.AsNoTracking().ToListAsync();

        return View(await model.ToListAsync());
    }
    enum sortOrder
    {
        title_asc = 0,
        title_desc,
        genre_desc,
        genre_asc
    }

    /// <summary>
    /// Show form to create a new book
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Success = false;
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
        ViewBag.Success = true;
        return View(book);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Delete(int? id)
    {
        ViewBag.BookId = id;
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
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
        ViewBag.Success = false;
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
                ViewBag.Success = true;        
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