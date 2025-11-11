using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IIntentoryService
    {
        Task savePurchase(Purchase obj);
        Task<IEnumerable<Purchase>> getallpurchase();
        Task<PagedResult<Purchase>> getallpurchase(int page, int pageSize);
        Task<IEnumerable<Sell>> getallsell();
        Task<PagedResult<Sell>> getallsell(int page, int pageSize);
        Task<string> saveNewSell(Sell sell);
        Task editSell(Sell obj);
        Task editPurchase(Purchase obj);
        Task<Sell> GetSellDetailsfromDB(long id);
    }
}