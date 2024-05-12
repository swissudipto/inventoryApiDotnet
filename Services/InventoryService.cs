using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using inventoryApiDotnet.Services;

namespace inventoryApiDotnet.Services
{
    public class InventoryService : IIntentoryService
    {
        public readonly MongoDBService _mongoDBService;
        public readonly IPurchaseRepository _purchaseRepository;

        public InventoryService(MongoDBService mongoDBService, IPurchaseRepository purchaseRepository)
        {
            _mongoDBService = mongoDBService;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<IActionResult> getallproducts(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> getallpurchase()
        {
            return await _purchaseRepository.GetAll();
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
            obj.Id = new Guid();
            _purchaseRepository.Add(obj);
            return obj;
        }
    }
}