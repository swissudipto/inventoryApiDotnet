using inventoryApiDotnet.Constants;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using MongoDB.Driver;

namespace inventoryApiDotnet.Services
{
    public class InventoryService : IIntentoryService
    {
        public readonly IPurchaseRepository _purchaseRepository;
        public readonly IStockservice _Stockservice;
        public readonly ISellRepository _sellRepository;
        public readonly IInvoiceCounterService _invoiceCounterService;
        public readonly IStockRepository _stockRepository;

        public InventoryService(IPurchaseRepository purchaseRepository,
                                IStockservice Stockservice,
                                ISellRepository sellRepository,
                                IInvoiceCounterService invoiceCounterService,
                                IStockRepository stockRepository)
        {
            _purchaseRepository = purchaseRepository;
            _Stockservice = Stockservice;
            _sellRepository = sellRepository;
            _invoiceCounterService = invoiceCounterService;
            _stockRepository = stockRepository;
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

        public async Task editPurchase(Purchase obj)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Purchase.Id),obj.Id??""}
              };
            var response = await _purchaseRepository.QueryCollectionAsync(new Purchase(), filterParameters);
            if (response != null && response.Count > 0)
            {
                var edititem = response.FirstOrDefault() ?? new Purchase();
                await stockModificationOnPurchaseItemChange(edititem.purchaseItems ?? new List<PurchaseItem>(),
                                                            obj.purchaseItems ?? new List<PurchaseItem>());
                edititem.purchaseItems = obj.purchaseItems;
                edititem.Comment = obj.Comment;
                edititem.InvoiceAmount = obj.InvoiceAmount;
                edititem.PurchaseDate = obj.PurchaseDate;
                edititem.SupplierAddress = obj.SupplierAddress;
                edititem.SupplierContactNumber = obj.SupplierContactNumber;
                edititem.SupplierName = obj.SupplierName;
                edititem.TotalAmount = obj.TotalAmount;
                await _purchaseRepository.Update(edititem);
            }
        }

        // This Method Require Modification. Unit of Work Should be implemented
        // Throgh Testing testing is required for this method
        public async Task<string> saveNewSell(Sell sell)
        {
            string message;
            foreach (SellItem item in sell.SellItems ?? new List<SellItem>())
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

        public async Task editSell(Sell obj)
        {
            var response = await GetSellDetailsfromDB(obj.Id ?? "");
            if (response != null && response.Count > 0)
            {
                // Stock modification logic should be Written here
                // By comparing old and new sellItems
                var edititem = response.FirstOrDefault() ?? new Sell();
                await stockModificationOnSellItemChange(edititem.SellItems ?? new List<SellItem>(),
                                                        obj.SellItems ?? new List<SellItem>());
                edititem.SellItems = obj.SellItems;
                edititem.Comment = obj.Comment;
                edititem.CustomerAddress = obj.CustomerAddress;
                edititem.CustomerName = obj.CustomerName;
                edititem.PhoneNumber = obj.PhoneNumber;
                edititem.SellDate = obj.SellDate;
                edititem.TotalAmount = obj.TotalAmount;
                await _sellRepository.Update(edititem);
            }
        }

        // Throgh Testing is required for this method
        private async Task stockModificationOnPurchaseItemChange(List<PurchaseItem> oldItems, List<PurchaseItem> newItems)
        {
            foreach (var item in newItems)
            {
                var matchedOldItem = oldItems.Where(y => y.ProductId == item.ProductId).FirstOrDefault();
                if (matchedOldItem != null)
                {
                    if (item.Quantity != matchedOldItem.Quantity)
                    {
                        var quantitydiffrence = item.Quantity - matchedOldItem.Quantity;
                        var filterParameters = new Dictionary<string, object>()
                        {
                            {nameof(Stock.ProductId),matchedOldItem.ProductId}
                        };
                        var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);

                        if (response != null)
                        {
                            var matchedStockitem = response?.FirstOrDefault() ?? new Stock();
                            matchedStockitem.Quantity = quantitydiffrence > 0 ? matchedStockitem.Quantity + quantitydiffrence : matchedStockitem.Quantity - item.Quantity;
                            await _stockRepository.Update(matchedStockitem);
                        }
                    }
                    oldItems.Remove(matchedOldItem);
                }
                else
                {
                    await _Stockservice.AddNewStock(item);
                }
            }

            foreach (var item in oldItems)
            {
                var filterParameters = new Dictionary<string, object>()
                    {
                       {nameof(Stock.ProductId),item.ProductId}
                    };
                var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);
                if (response != null)
                {
                    var matchedOldItem = response.FirstOrDefault();
                    matchedOldItem.Quantity -= item.Quantity;
                    await _stockRepository.Update(matchedOldItem);
                }
            }
        }

        // Throgh Testing is required for this method
        private async Task stockModificationOnSellItemChange(List<SellItem> oldItems, List<SellItem> newItems)
        {
            foreach (var item in newItems)
            {
                var matchedOldItem = oldItems.Where(y => y.ProductId == item.ProductId).FirstOrDefault();
                if (matchedOldItem != null)
                {
                    if (item.Quantity != matchedOldItem.Quantity)
                    {
                        var quantitydiffrence = matchedOldItem.Quantity - item.Quantity;
                        var filterParameters = new Dictionary<string, object>()
                    {
                       {nameof(Stock.ProductId),matchedOldItem.ProductId}
                    };
                        var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);
                        if (response != null)
                        {
                            var matchedStockitem = response?.FirstOrDefault() ?? new Stock();
                            matchedStockitem.Quantity += quantitydiffrence;
                            await _stockRepository.Update(matchedStockitem);
                        }
                    }
                    oldItems.Remove(matchedOldItem);
                }
                else
                {
                    string message;
                    var response = _Stockservice.checkIfProductInStock(item, out message);
                    if (response)
                    {
                        await _Stockservice.afterSellStockModification(item);
                    }
                }
            }

            foreach (var item in oldItems)
            {
                var filterParameters = new Dictionary<string, object>()
                    {
                       {nameof(Stock.ProductId),item.ProductId}
                    };
                var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);
                if (response != null)
                {
                    var matchedOldItem = response.FirstOrDefault();
                    matchedOldItem.Quantity += item.Quantity;
                    await _stockRepository.Update(matchedOldItem);
                }
            }
        }

        public async Task<List<Sell>> GetSellDetailsfromDB(string id)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Purchase.Id),id ?? ""}
              };
            return await _sellRepository.QueryCollectionAsync(new Sell(), filterParameters);
        }
    }
}