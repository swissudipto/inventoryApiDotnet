using MongoDB.Driver;
using inventoryApiDotnet.Interface;
using ServiceStack;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext Context;
        protected DbSet<TEntity> DbSet;

        protected BaseRepository(AppDbContext context)
        {
            Context = context;

            DbSet = Context.Set<TEntity>();
        }
        // ---------------------- Add ----------------------
        public virtual async Task Add(TEntity obj)
        {
            await DbSet.AddAsync(obj);
        }

        public virtual async Task AddRange(List<TEntity> obj)
        {
            await DbSet.AddRangeAsync(obj);
        }

        // ---------------------- GetById ----------------------
        public virtual async Task<TEntity?> GetById(long id)
        {
            return await DbSet.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        // ---------------------- GetAll ----------------------
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        // ---------------------- Update ----------------------
        public virtual async Task Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        // ---------------------- Remove ----------------------
        public virtual void Remove(Guid id)
        {
            var entity = DbSet.Local.FirstOrDefault(e => EF.Property<Guid>(e, "Id") == id);
            if (entity == null)
            {
                entity = Activator.CreateInstance<TEntity>();
                var property = typeof(TEntity).GetProperty("Id");
                property?.SetValue(entity, id);
                Context.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                DbSet.Remove(entity);
            }
        }

        // ---------------------- QueryCollectionAsync ----------------------
        public async Task<List<TEntity>> QueryCollectionAsync(
            TEntity obj,
            Dictionary<string, object> filterParameters)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var parameter in filterParameters)
            {
                var property = typeof(TEntity).GetProperty(parameter.Key);
                if (property != null)
                {
                    query = query.Where(e =>
                        EF.Property<object>(e, parameter.Key).Equals(parameter.Value));
                }
            }

            return await query.ToListAsync();
        }

        // ---------------------- GetAllbyPage ----------------------
        public async Task<IEnumerable<TEntity>> GetAllbyPage(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var entityType = typeof(TEntity);
            var prop = entityType.GetProperty("TransactionDateTime") ?? entityType.GetProperty("transactionDateTime");

            if (prop != null)
            {
                // order dynamically by transactionDateTime descending
                var query = DbSet.OrderByDescending(e => EF.Property<DateTime?>(e, prop.Name));
                return await query.Skip(skip).Take(pageSize).ToListAsync();
            }

            // fallback: no sorting
            return await DbSet.Skip(skip).Take(pageSize).ToListAsync();
        }

        // ---------------------- Dispose ----------------------
        public void Dispose()
        {
            Context?.Dispose();
        }

        // ---------------------- GetCollectionCount ----------------------
        public async Task<long> GetCollectionCount()
        {
            var count = await DbSet.CountAsync();
            return count;
        }
    }
}
