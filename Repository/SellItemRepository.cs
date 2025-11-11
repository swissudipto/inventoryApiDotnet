using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class SellItemRepository : BaseRepository<SellItem>, ISellItemRepository
    {
        public SellItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
