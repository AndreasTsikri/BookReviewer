using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SQLitePCL;

namespace BookReviewer.Services;
public class ReviewsService : IService<Review>
{
    //ApplicationDbContext _ctx;
    ILogger<ReviewsService> _lgr;
    readonly UnitOfWork _uot;
    // public ReviewsService(Logger<ReviewsService> lgr, ApplicationDbContext appDbContext){
    //     this._ctx = appDbContext;
    //     this._lgr = lgr;
    // }
    public ReviewsService(
        ILogger<ReviewsService> lgr, IUnitOfWork uot)
    {
        //this._ctx = appDbContext;
        this._lgr = lgr;
        this._uot = uot as UnitOfWork ?? throw new InvalidOperationException("Injected UoW is not the concrete type.");


    }

    public async Task<bool> Create(Review r)
    {
        try
        {
           await _uot.ReviewRepo.AddEntity(r); // await _ctx.Reviews.AddAsync(r);
            await _uot.SaveAsync();// await _ctx.SaveChangesAsync();            
        }
        catch
        {
            //something wrong - let client hanlde
            _lgr.Log(LogLevel.Error, $"Problem inserting review {r.Id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Update(Review r)
    {
        try
        {
            // Review? r = await GetItemById(id);
            // if(r == null)
            //     return false;
            //_ctx.Update(r);
            //await _ctx.SaveChangesAsync();
            _uot.ReviewRepo.UpdateEntity(r);
            await _uot.SaveAsync();         
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem updating review {r.Id}");
            return false;
        }
        return true;
    }

    public async Task<bool> Delete(Review r)
    {
        try
        {
            // Review? r = await GetItemById(id);
            // if(r == null)
            //     return false;
            // _ctx.Reviews.Remove(r);
            // await _ctx.SaveChangesAsync(); 
            _uot.ReviewRepo.DeleteEntity(r);
            await _uot.SaveAsync();
        }
        catch
        {
            //something wrong - let client handle
            _lgr.Log(LogLevel.Error, $"Problem deleting review {r.Id}");
            return false;
        }
        return true;
    }
    public async Task<IEnumerable<Review>> GetValues() => await _uot.ReviewRepo.GetValues().ToListAsync();//await _ctx.Reviews.ToListAsync();
    public async Task<Review?> GetItemById(int id) => await _uot.ReviewRepo.GetById(id);//await _ctx.Reviews.FindAsync(id);
    public IQueryable<Review> GetQueryableNoUpdate() => _uot.ReviewRepo.GetValues();//_ctx.Reviews.AsNoTracking();
}