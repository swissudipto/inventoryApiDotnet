using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;

namespace inventoryApiDotnet.Repository
{
    public class SellRepository : BaseRepository<Sell>, ISellRepository
    {
        public SellRepository(IMongoContext context) : base(context)
        {
        }
    }
}
