using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(IMongoContext context) : base(context)
        {
        }
    }
}
