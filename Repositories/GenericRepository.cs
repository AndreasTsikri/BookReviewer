using System.Threading.Tasks;
using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity:class
{
    public ApplicationDbContext _ctx;
    public DbSet<TEntity> set;
    public GenericRepository(ApplicationDbContext ctx){
        this._ctx = ctx;
        this.set = _ctx.Set<TEntity>();
    }
    public IQueryable<TEntity> GetValues() => this.set.AsNoTracking();//ToListAsync();
    public async Task<TEntity?> GetById(int id) => await this.set.FindAsync(id);
    public async Task AddEntity(TEntity obj) => await this.set.AddAsync(obj);
    public void UpdateEntity(TEntity obj) => this.set.Update(obj);
    public void DeleteEntity(TEntity obj) => this.set.Remove(obj);
}