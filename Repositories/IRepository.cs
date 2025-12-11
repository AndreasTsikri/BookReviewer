public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetValues();
    //Task<IEnumerable<TEntity>> GetValuesListAsync();
    Task<TEntity?> GetById(int id);
    Task AddEntity(TEntity obj);
    void UpdateEntity(TEntity obj);
    void DeleteEntity(TEntity obj);
}