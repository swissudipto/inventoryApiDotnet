using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inventoryApiDotnet.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<bool> Update(TEntity obj);
        void Remove(Guid id);
        Task<long> GetCollectionCount();
        Task<List<TEntity>> QueryCollectionAsync(TEntity obj,Dictionary<string, object> filterParameters);
        Task<IEnumerable<TEntity>> GetAllbyPage(int page, int pageSize);
    }
}