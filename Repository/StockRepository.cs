using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        protected DbSet<Stock> _DbSet;

        public StockRepository(AppDbContext context) : base(context)
        {
            _DbSet = Context.Set<Stock>();

        }

        public async Task<Stock> GetByProductId(long productId)
        {
            return _DbSet.FirstOrDefault(x => x.ProductId == productId);
        }
    }
}
