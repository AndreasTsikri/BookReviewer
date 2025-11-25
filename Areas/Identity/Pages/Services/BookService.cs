using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReviewer.Services;

public class BookService
{
    ApplicationDbContext _ctx;
    public BookService(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Book>> GetBooksAsync() => await _ctx.Books.AsNoTracking().ToListAsync();
    public async Task<List<Book>> GetBooksWithFiltersAsync(string tso, string gso, string aso, string searchStr)
    {
        bool isAsc(string s) => s == "asc";
        var m = _ctx.Books.AsNoTracking();
        if (tso != null)
            m = isAsc(tso!) ? m.OrderBy(b => b.Title) : m.OrderByDescending(b => b.Title);
        if (gso != null)
            m = isAsc(gso!) ? m.OrderBy(b => b.Genre) : m.OrderByDescending(b => b.Genre);
        if (aso != null)
            m = isAsc(aso!) ? m.OrderBy(b => b.Author) : m.OrderByDescending(b => b.Author);

        if (!string.IsNullOrEmpty(searchStr))
            m = m.Where(b => b.Title.Contains(searchStr));
        return await m.ToListAsync();
    }
    public async Task<Book?> GetBookAsync(int id) => await _ctx.Books.FindAsync(id);
    public async Task<Book> SetBookAsync(Book book)
    {
        await _ctx.Books.AddAsync(book);
        await _ctx.SaveChangesAsync();
        return book;
    }
    // public async Task<Book> UpdateBookAsync(Book newBook)
    // {
    // }
    public async Task<Book?> DeleteBook(int id)
    {
        var b = await _ctx.Books.FindAsync(id);
        if (b != null)
        {
            _ctx.Books.Remove(b);
            await _ctx.SaveChangesAsync();
        }
        return b;
}
}