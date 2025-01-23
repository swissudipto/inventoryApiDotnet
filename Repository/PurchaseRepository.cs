using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;
using MongoDB.Driver;
using MongoDB.Bson;

namespace inventoryApiDotnet.Repository
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        protected IMongoCollection<Purchase> _collection;
        public PurchaseRepository(IMongoContext context) : base(context)
        {
            _collection = Context.GetCollection<Purchase>(nameof(Purchase));
        }

        // public async Task<IEnumerable<Purchase>> GetAllbyPage(int page, int pageSize)
        // {
        //     var skip = (page - 1) * pageSize;
        //     return await _collection
        //             .Find(x => true)
        //             .Skip(skip)
        //             .Limit(pageSize)
        //             .ToListAsync();
        // }
    }
}
