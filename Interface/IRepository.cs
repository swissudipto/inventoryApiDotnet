namespace inventoryApiDotnet.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity obj);
        Task AddRange(List<TEntity> obj);
        Task<TEntity> GetById(long id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity obj);
        void Remove(Guid id);
        Task<long> GetCollectionCount();
        Task<List<TEntity>> QueryCollectionAsync(TEntity obj,Dictionary<string, object> filterParameters);
        Task<IEnumerable<TEntity>> GetAllbyPage(int page, int pageSize);      
    }
}