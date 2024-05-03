using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;

namespace inventoryApiDotnet.Services
{
    public class InventoryService : IIntentoryService
    {
        public async Task<IActionResult> getallproducts(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> getallpurchase(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> getallsell(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> getallStock(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<Purchase> savePurchase(Purchase obj)
        {
            return obj;
        }
    }
}