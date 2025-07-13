using MongoDB.Driver;
using inventoryApiDotnet.Interface;
using ServiceStack;
using MongoDB.Bson;

namespace inventoryApiDotnet.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;

            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task Add(TEntity obj)
        {
            await DbSet.InsertOneAsync(obj);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        { 
           return await DbSet.Find(new BsonDocument()).ToListAsync();
        }

        public virtual async Task<bool> Update(TEntity obj)
        {
            var result = await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj);
            return result.IsAcknowledged ? result.IsAcknowledged : false;
        }

        public virtual void Remove(Guid id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public async Task<List<TEntity>> QueryCollectionAsync(TEntity obj,
                                                              Dictionary<string, object> filterParameters)
        {
            // Build the filter
            var filterBuilder = Builders<TEntity>.Filter;
            var filter = FilterDefinition<TEntity>.Empty;

            foreach (var parameter in filterParameters)
            {
                // Add each filter parameter to the filter
                filter &= filterBuilder.Eq(parameter.Key, BsonValue.Create(parameter.Value));
            }

            // Query the collection with the constructed filter
            var results = await DbSet.Find(filter).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<TEntity>> GetAllbyPage(int page, int pageSize)
        {
            var sort = Builders<TEntity>.Sort.Descending("transactionDateTime");
            var skip = (page - 1) * pageSize;
            return await DbSet
                    .Find(x => true)
                    .Sort(sort)
                    .Skip(skip)
                    .Limit(pageSize)
                    .ToListAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
        public async Task<long> GetCollectionCount()
        {
            return  await DbSet.CountDocumentsAsync(Builders<TEntity>.Filter.Empty);
        }
    }
}
