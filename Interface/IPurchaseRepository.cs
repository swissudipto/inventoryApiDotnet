using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<IEnumerable<Purchase>> GetAllbyPageWithItems(int page, int pageSize);
        Task<Purchase> GetByPuchaseId(long purchaseId);
    }
}