using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface ISellRepository : IRepository<Sell>
    {
        Task<IEnumerable<Sell>> GetAllbyPageWithItems(int page, int pageSize);
    }
}