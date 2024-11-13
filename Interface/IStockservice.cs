using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IStockservice
    {
        Task<string> AddNewStock(Purchase obj); 
        Task<List<Stock>> GetAllStock();
        Boolean checkIfProductInStock(Sell sell, out string meassage);
        Task afterSellStockModification(Sell sell);
    }
}