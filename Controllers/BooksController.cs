using BookReviewer.Data;
using BookReviewer.Models;
using  BookReviewer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
namespace BookReviewer.Controllers;

[Authorize]
public class BooksController : Controller
{
    //readonly ILogger<BooksController> _logger;
    //readonly ApplicationDbContext _ctx;
    //readonly IMapper _mapper;
    readonly IService<Book> _bs;

    public BooksController(IService<Book> bs/*, IMapper mapper*/)
    {
        this._bs = bs;        
    }

    [HttpGet]
   
    public async Task<IActionResult> Index(string tso, string gso, string aso, string searchStr)
    {
        ViewBag.TitleSortParm = tso == "desc" ? "asc" : "desc";
        ViewBag.GenreSortParam = gso == "desc" ? "asc" : "desc";
        ViewBag.AuthorSortParm = aso == "desc" ? "asc" : "desc";
        ViewBag.SearchStr = !string.IsNullOrEmpty(searchStr) ? searchStr : "";
        
        var tbs = this._bs as BookService ?? throw new InvalidCastException("Problem when casting book interface");
        var books = await tbs.FilterAndSort(tso,gso, aso, searchStr);

        return View(books);
    }

    /// <summary>
    /// Show form to create a new book
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Success = false;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Title, Author, PublishedYear, Genre")] Book book)
    {
        if (!ModelState.IsValid)
            return View(book);

        await _bs.Create(book);

        ViewBag.Success = true;
        return View(book);
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        ViewBag.BookId = id;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var b = await _bs.GetItemById(id);
        if(b == null)
            return NotFound();
        
        if(!await _bs.Delete(b))
            return StatusCode(500, $"Problem deleting the {id}");
        
        return RedirectToAction("Index");
    }

    // GET: Movies/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Success = false;
        var b = await _bs.GetItemById(id);
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PublishedYear")] Book book)// TODO : Here replace Book with BookViewModel and delete binding
    {
        if (id != book.Id )
            return BadRequest();

        var b = await _bs.GetItemById(id);
        if(b == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View(book);
            
        b.Title = book.Title;
        b.Author = book.Author;
        b.Genre = book.Genre;
        b.PublishedYear = book.PublishedYear;

        if(!await _bs.Update(b))
            return StatusCode(500, "Problem editing the book ");
           
        return RedirectToAction(nameof(Index));
    }

}