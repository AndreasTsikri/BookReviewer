using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

public class BookService : IService<Book>
{
    // ApplicationDbContext _ctx;
    // Logger<BookService> _lgr;
    // public BookService(Logger<BookService> lgr, ApplicationDbContext appDbContext){
    //     this._ctx = appDbContext;
    //     this._lgr = lgr;
    // }
    readonly UnitOfWork _uot;
     ILogger<BookService> _lgr;
    // ApplicationDbContext _ctx;
    //  public BookService(Logger<BookService> lgr, ApplicationDbContext appDbContext){
    //      this._ctx = appDbContext;
    //      this._lgr = lgr;
    //  }
    public BookService(ILogger<BookService> lgr, IUnitOfWork uot){
          this._lgr = lgr;
          this._uot = uot as UnitOfWork 
          ?? throw new InvalidOperationException("Injected UoW is not the concrete type.");;
      }

    public async Task<bool> Create(Book b)
    {
        try
        {
            await _uot.BookRepo.AddEntity(b);//_ctx.Books.AddAsync(b);
            await _uot.SaveAsync<Book>();//_ctx.SaveChangesAsync();            
        }
        catch
        {
            //something wrong - let client hanlde
            _lgr.Log(LogLevel.Error, $"Problem inserting book {b.Id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Update(Book b)
    {
        try
        {
            _uot.BookRepo.UpdateEntity(b);// _ctx.Update(b);
            await _uot.SaveAsync<Book>();// await _ctx.SaveChangesAsync();         
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem updating book {b.Id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Delete(Book b)
    {
        try
        {
            _uot.BookRepo.DeleteEntity(b);
            await _uot.SaveAsync<Book>();         
            //_ctx.Books.Remove(b);
            //await _ctx.SaveChangesAsync();         
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem deleting book {b.Id}");
            return false;
        }
        return true;
    }


    public async Task<IEnumerable<Book>> GetValues() => await _uot.BookRepo.GetValues().ToListAsync();//await _ctx.Books.ToListAsync();;

    public IQueryable<Book> GetQueryableNoUpdate() => _uot.BookRepo.GetValues();//_ctx.Books.AsNoTracking();

    public async Task<Book?> GetItemById(int id) => await _uot.BookRepo.GetById(id);//_ctx.Books.FindAsync(id);

    public async Task<List<Book>> FilterAndSort(string tso, string gso, string aso, string searchStr)
    {
        bool isAsc(string s) => s == "asc";

        var books = GetQueryableNoUpdate();

        if (tso != null)
            books = isAsc(tso!) ? books.OrderBy(b => b.Title) : books.OrderByDescending(b => b.Title);
        if (gso != null)
            books = isAsc(gso!) ? books.OrderBy(b => b.Genre) : books.OrderByDescending(b => b.Genre);
        if (aso != null)
            books = isAsc(aso!) ? books.OrderBy(b => b.Author) : books.OrderByDescending(b => b.Author);

        if (!string.IsNullOrEmpty(searchStr))
            books = books.Where(b => b.Title.Contains(searchStr));
        return await books.ToListAsync();
    }
}

enum sortOrder
    {
        title_asc = 0,
        title_desc,
        genre_desc,
        genre_asc
    }