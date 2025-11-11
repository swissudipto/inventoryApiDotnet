using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class InvoiceCounterRepository : BaseRepository<InvoiceCounter>, IInvoiceCounterRepository
    {
        public InvoiceCounterRepository(AppDbContext context) : base(context)
        {
        }
    }
}
