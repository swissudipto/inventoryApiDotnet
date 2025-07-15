using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class SellRepository : BaseRepository<Sell>, ISellRepository
    {
        public SellRepository(IMongoContext context) : base(context)
        {
        }
    }
}
