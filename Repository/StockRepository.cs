using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;

namespace inventoryApiDotnet.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(IMongoContext context) : base(context)
        {
        }
    }
}
