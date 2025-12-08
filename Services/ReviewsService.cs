using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SQLitePCL;

namespace BookReviewer.Services;
public class ReviewsService : IService<Review>
{
    ApplicationDbContext _ctx;
    Logger<ReviewsService> _lgr;
    public ReviewsService(Logger<ReviewsService> lgr, ApplicationDbContext appDbContext){
        this._ctx = appDbContext;
        this._lgr = lgr;
    }

    public async Task<bool> Create(Review r)
    {
        try
        {
            await _ctx.Reviews.AddAsync(r);
            await _ctx.SaveChangesAsync();            
        }
        catch
        {
            //something wrong - let client hanlde
            _lgr.Log(LogLevel.Error, $"Problem inserting review {r.Id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Update(int id)
    {
        try
        {
            Review? r = await GetItemById(id);
            if(r == null)
                return false;
            _ctx.Update(r);
            await _ctx.SaveChangesAsync();         
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem updating review {id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            Review? r = await GetItemById(id);
            if(r == null)
                return false;
            _ctx.Reviews.Remove(r);
            await _ctx.SaveChangesAsync();         
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem deleting review {id}");
            return false;
        }
        return true;
    }
    public async Task<List<Review>> GetValues()
    {
        var l = await _ctx.Reviews.ToListAsync();
        return l;
    }
    public async Task<Review?> GetItemById(int id)
    {
        var b = await _ctx.Reviews.FindAsync(id);
        return b;
    }
    public IQueryable<Review> GetQueryableNoUpdate()
    {
        var l = _ctx.Reviews.AsNoTracking();
        return l;
    }
}