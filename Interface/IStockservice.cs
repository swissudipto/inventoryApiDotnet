using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IStockservice
    {
        Task<string> AddNewStock(PurchaseItem obj);
        Task<List<Stock>> GetAllStock(); 
        Task<List<Stock>> GetAllStock(int page, int pageSize);
        Boolean checkIfProductInStock(Sell sell, out string meassage);
        Task afterSellStockModification(Sell sell);
    }
}