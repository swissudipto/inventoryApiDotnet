using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;

namespace inventoryApiDotnet.Repository
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(IMongoContext context) : base(context)
        {
        }
    }
}
