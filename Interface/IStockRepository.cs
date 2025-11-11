using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> GetByProductId(long productId);
    }
}