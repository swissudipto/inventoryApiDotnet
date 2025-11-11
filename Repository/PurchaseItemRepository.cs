using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class PurchaseItemRepository : BaseRepository<PurchaseItem>, IPurchaseItemRepository
    {
        public PurchaseItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
