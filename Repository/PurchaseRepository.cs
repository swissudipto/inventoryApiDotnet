using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        protected DbSet<Purchase> _DbSet;
        public PurchaseRepository(AppDbContext context) : base(context)
        {
            _DbSet = Context.Set<Purchase>();
        }

        public async Task<IEnumerable<Purchase>> GetAllbyPageWithItems(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var entityType = typeof(Purchase);
            var prop = entityType.GetProperty("TransactionDateTime") ?? entityType.GetProperty("transactionDateTime");

            if (prop != null)
            {
                // order dynamically by transactionDateTime descending
                var query = DbSet.Include(x => x.purchaseItems).OrderByDescending(e => EF.Property<DateTime?>(e, prop.Name));
                return await query.Skip(skip).Take(pageSize).ToListAsync();
            }

            // fallback: no sorting
            return await DbSet.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<Purchase> GetByPuchaseId(long purchaseId)
        {
            return await _DbSet.Include(a => a.purchaseItems)
                               .Where(x => x.PurchaseId == purchaseId).FirstOrDefaultAsync();
        }
    }
}
