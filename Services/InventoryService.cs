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
        public readonly IInvoiceCounterService _invoiceCounterService;

        public InventoryService(MongoDBService mongoDBService,
                                IPurchaseRepository purchaseRepository,
                                IStockservice Stockservice,
                                IProductService productService,
                                ISellRepository sellRepository,
                                IInvoiceCounterService invoiceCounterService)
        {
            _mongoDBService = mongoDBService;
            _purchaseRepository = purchaseRepository;
            _Stockservice = Stockservice;
            _productservice = productService;
            _sellRepository = sellRepository;
            _invoiceCounterService = invoiceCounterService;
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

        public async Task<IEnumerable<Purchase>> getallpurchase(int page, int pageSize)
        {
            var allPurchaseList = await _purchaseRepository.GetAllbyPage(page,pageSize);
            return allPurchaseList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<IEnumerable<Sell>> getallsell()
        {
            var allSellList = await _sellRepository.GetAll();
            return allSellList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<IEnumerable<Sell>> getallsell(int page, int pageSize)
        {
            var allSellList = await _sellRepository.GetAllbyPage(page,pageSize);
            return allSellList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<IActionResult> getallStock(Purchase obj)
        {
            throw new NotImplementedException();
        }

        public async Task savePurchase(Purchase obj)
        {
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
            sell.InvoiceNo = await _invoiceCounterService.GenerateInvoiceNumber();
            sell.transactionDateTime = DateTime.Now;
            await _sellRepository.Add(sell);
            await _Stockservice.afterSellStockModification(sell);
            return message;
        }
    }
}