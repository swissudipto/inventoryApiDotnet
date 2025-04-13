using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        Task<IActionResult> getallStock(Purchase obj);
        Task<IEnumerable<Sell>> getallsell();
        Task<IEnumerable<Sell>> getallsell(int page, int pageSize);
        Task<string> saveNewSell(Sell sell);
    }
}