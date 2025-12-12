using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public interface IUnitOfWork
{
    Task SaveAsync<T>();
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

    public async Task SaveAsync<T>() {

        bool saved = false;
        while (!saved)
        {
            try
            {
                await this._ctx.SaveChangesAsync();
                saved = true;
            }
            catch( DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
            {
                if (entry.Entity is T)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues!= null ? 
                        databaseValues[property] : null;

                        //value to be saved on db after conflict
                        proposedValues[property] = proposedValue;// this can be "databaseValue" also!
                    }
                    // Refresh original values to bypass next concurrency check
                    if(databaseValues != null)
                        entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
    }
    }

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