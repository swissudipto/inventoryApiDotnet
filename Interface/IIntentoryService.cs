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
        Task<Purchase> savePurchase(Purchase obj);
        Task<IActionResult> getallpurchase(Purchase obj);
        Task<IActionResult> getallproducts(Purchase obj);
        Task<IActionResult> getallStock(Purchase obj);
        Task<IActionResult> getallsell(Purchase obj);
    }
}