using inventoryApiDotnet.Constants;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace inventoryApiDotnet.Services
{
    public class InventoryService : IIntentoryService
    {
        public readonly IPurchaseRepository _purchaseRepository;
        public readonly IStockservice _Stockservice;
        public readonly ISellRepository _sellRepository;
        public readonly IInvoiceCounterService _invoiceCounterService;

        public InventoryService(IPurchaseRepository purchaseRepository,
                                IStockservice Stockservice,
                                ISellRepository sellRepository,
                                IInvoiceCounterService invoiceCounterService)
        {
            _purchaseRepository = purchaseRepository;
            _Stockservice = Stockservice;
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

        public async Task<PagedResult<Purchase>> getallpurchase(int page, int pageSize)
        {
            var allPurchaseList = await _purchaseRepository.GetAllbyPage(page, pageSize);
            var totalRecords = await _purchaseRepository.GetCollectionCount();

            return new PagedResult<Purchase>(allPurchaseList.ToList(), totalRecords, page, pageSize);
        }

        public async Task<IEnumerable<Sell>> getallsell()
        {
            var allSellList = await _sellRepository.GetAll();
            return allSellList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<PagedResult<Sell>> getallsell(int page, int pageSize)
        {
            var allSellList = await _sellRepository.GetAllbyPage(page, pageSize);
            var totalRecords = await _sellRepository.GetCollectionCount();
            return new PagedResult<Sell>(allSellList.ToList(), totalRecords, page, pageSize);
        }

        public async Task savePurchase(Purchase obj)
        {
            obj.transactionDateTime = DateTime.Now;
            obj.PurchaseId = await _purchaseRepository.GetCollectionCount() + 1;
            await _purchaseRepository.Add(obj);
            obj.purchaseItems?.ForEach(item => _Stockservice.AddNewStock(item));
        }

        // This Method Require Modification. Unit of Work Should be implemented
        // Throgh Testing testing is required for this method
        public async Task<string> saveNewSell(Sell sell)
        {
            string message;
            foreach (SellItem item in sell.SellItems)
            {
                var response = _Stockservice.checkIfProductInStock(item, out message);
                if (!response)
                {
                    return message;
                }
                await _Stockservice.afterSellStockModification(item);
            }

            sell.InvoiceNo = await _invoiceCounterService.GenerateInvoiceNumber();
            sell.transactionDateTime = DateTime.Now;
            await _sellRepository.Add(sell);
            return InventoryConstants.SuccessMessage;
        }
    }
}