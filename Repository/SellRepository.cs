using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public class SellRepository : BaseRepository<Sell>, ISellRepository
    {
        protected DbSet<Sell> _DbSet;
        public SellRepository(AppDbContext context) : base(context)
        {
             _DbSet = Context.Set<Sell>();
        }

        public async Task<IEnumerable<Sell>> GetAllbyPageWithItems(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var entityType = typeof(Sell);
            var prop = entityType.GetProperty("TransactionDateTime") ?? entityType.GetProperty("transactionDateTime");

            if (prop != null)
            {
                // order dynamically by transactionDateTime descending
                var query = DbSet.Include(x => x.SellItems).OrderByDescending(e => EF.Property<DateTime?>(e, prop.Name));
                return await query.Skip(skip).Take(pageSize).ToListAsync();
            }

            // fallback: no sorting
            return await DbSet.Skip(skip).Take(pageSize).ToListAsync();
        }
    }
}
