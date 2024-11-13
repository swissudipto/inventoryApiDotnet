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
        public readonly IStockservice _Stockservice;
        public readonly IProductService _productservice;
        public readonly ISellRepository _sellRepository;

        public InventoryService(MongoDBService mongoDBService,
                                IPurchaseRepository purchaseRepository,
                                IStockservice Stockservice,
                                IProductService productService,
                                ISellRepository sellRepository)
        {
            _mongoDBService = mongoDBService;
            _purchaseRepository = purchaseRepository;
            _Stockservice = Stockservice;
            _productservice = productService;
            _sellRepository = sellRepository;
        }

        public async Task<IActionResult> getallproducts(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> getallpurchase()
        {
            var allPurchaseList = await _purchaseRepository.GetAll();
            return allPurchaseList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<IEnumerable<Sell>> getallsell()
        {
            var allSellList = await _sellRepository.GetAll();
            return allSellList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<IActionResult> getallStock(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task savePurchase(Purchase obj)
        {
            Random rand = new Random();
            obj.PurchaseId = string.Concat("PR" + rand.Next(0000, 9999) + (_purchaseRepository.GetCollectionCount() + 1));
            var product = await _productservice.GetProductById((long)obj.ProductId);
            obj.ProductName = product.ProductName;
            obj.transactionDateTime = DateTime.Now;
            await _purchaseRepository.Add(obj);
            await _Stockservice.AddNewStock(obj);
        }
        
        public async Task<string> saveNewSell(Sell sell)
        {
            string message;
            var response = _Stockservice.checkIfProductInStock(sell, out message);
            if (!response)
            {
                return message;
            }
            sell.InvoiceNo = "010101010"; // Create a Invoice e Generation Logic 
            sell.transactionDateTime = DateTime.Now;
            await _sellRepository.Add(sell);
            await _Stockservice.afterSellStockModification(sell);
            return message;
        }
    }
}