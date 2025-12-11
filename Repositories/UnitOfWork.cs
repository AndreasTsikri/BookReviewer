using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public interface IUnitOfWork
{
    Task SaveAsync();
    void Dispose();
    
}
public class UnitOfWork: IUnitOfWork, IDisposable
{
    readonly IRepository<Book> _bookRepo;
    readonly IRepository<Review> _reviewsRepo;
    readonly ApplicationDbContext _ctx;
    public UnitOfWork(ApplicationDbContext ctx)
    {
        this._bookRepo    = new GenericRepository<Book>(ctx);
        this._reviewsRepo = new GenericRepository<Review>(ctx);
        this._ctx = ctx;
    } 
    public UnitOfWork(ApplicationDbContext ctx, IRepository<Book> br, IRepository<Review> rr)
    {
        this._bookRepo    = br;
        this._reviewsRepo = rr;
        this._ctx = ctx;
    } 
    public IRepository<Book> BookRepo
    {
        get => this._bookRepo;
    }
     public IRepository<Review> ReviewRepo
    {
        get => this._reviewsRepo;
    }

    public async Task SaveAsync() => await this._ctx.SaveChangesAsync();
    

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
}