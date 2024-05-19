using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        public async Task savePurchase(Purchase obj)
        {
            Random rand = new Random();
            obj.Id = ObjectId.GenerateNewId().ToString();
            obj.InvoiceNo = string.Concat ("PR" + rand.Next(0000,9999) +(_purchaseRepository.GetCollectionCount()+1));
            await _purchaseRepository.Add(obj);
        }
    }
}