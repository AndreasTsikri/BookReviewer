using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IService<T>
{
    public Task<bool> Create(T item);
    public Task<bool> Update(T item);
    public Task<bool> Delete(T item);
    public Task<IEnumerable<T>> GetValues();
    public IQueryable<T> GetQueryableNoUpdate();
    Task<T?> GetItemById(int id);
}