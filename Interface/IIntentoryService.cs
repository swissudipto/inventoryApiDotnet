using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;

namespace inventoryApiDotnet.Interface
{
    public interface IIntentoryService
    {
        Task savePurchase(Purchase obj);
        Task<IEnumerable<Purchase>> getallpurchase();
        Task<PagedResult<Purchase>> getallpurchase(int page, int pageSize);
        Task<IActionResult> getallproducts(Purchase obj);
        Task<IEnumerable<Sell>> getallsell();
        Task<PagedResult<Sell>> getallsell(int page, int pageSize);
        Task<string> saveNewSell(Sell sell);
    }
}