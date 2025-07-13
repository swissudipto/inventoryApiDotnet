using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IStockservice
    {
        Task<string> AddNewStock(PurchaseItem obj);
        Task<List<Stock>> GetAllStock();
        Task<List<Stock>> GetAllStock(int page, int pageSize);
        Boolean checkIfProductInStock(SellItem sellItem, out string meassage);
        Task afterSellStockModification(SellItem sellItems);
        Task<Stock> GetStockByProductId(long productId);
    }
}