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
        public readonly IPurchaseItemRepository _purchaseItemRepository;
        public readonly ISellItemRepository _sellItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IPurchaseRepository purchaseRepository,
                                IStockservice Stockservice,
                                ISellRepository sellRepository,
                                IInvoiceCounterService invoiceCounterService,
                                IStockRepository stockRepository,
                                IPurchaseItemRepository purchaseItemRepository,
                                ISellItemRepository sellItemRepository,
                                IUnitOfWork unitOfWork)
        {
            _purchaseRepository = purchaseRepository;
            _Stockservice = Stockservice;
            _sellRepository = sellRepository;
            _invoiceCounterService = invoiceCounterService;
            _stockRepository = stockRepository;
            _purchaseItemRepository = purchaseItemRepository;
            _sellItemRepository = sellItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Purchase>> getallpurchase()
        {
            var allPurchaseList = await _purchaseRepository.GetAll();
            return allPurchaseList.OrderByDescending(x => x.transactionDateTime).ToList();
        }

        public async Task<PagedResult<Purchase>> getallpurchase(int page, int pageSize)
        {
            var allPurchaseList = await _purchaseRepository.GetAllbyPageWithItems(page, pageSize);
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
            var allSellList = await _sellRepository.GetAllbyPageWithItems(page, pageSize);
            var totalRecords = await _sellRepository.GetCollectionCount();
            return new PagedResult<Sell>(allSellList.ToList(), totalRecords, page, pageSize);
        }

        public async Task savePurchase(Purchase obj)
        {
            obj.transactionDateTime = DateTime.UtcNow;
            obj.PurchaseId = await _purchaseRepository.GetCollectionCount() + 1;
            obj.purchaseItems.ToList().ForEach(x => { x.PurchaseId = obj.PurchaseId; });
            await _purchaseRepository.Add(obj);
            await _Stockservice.AddNewStock(obj.purchaseItems.ToList());
            await _unitOfWork.SaveAsync();
        }

        public async Task editPurchase(Purchase obj)
        {
            var newPurchase = obj.purchaseItems;
            var existingPurchase = await _purchaseRepository.GetByPuchaseId(obj.PurchaseId ?? 0);
            if (existingPurchase != null)
            {
                await stockModificationOnPurchaseItemChange(existingPurchase.purchaseItems.ToList() ?? new List<PurchaseItem>(),
                                                            newPurchase.ToList() ?? new List<PurchaseItem>());
                existingPurchase.Comment = obj.Comment;
                existingPurchase.PurchaseDate = obj.PurchaseDate;
                existingPurchase.SupplierAddress = obj.SupplierAddress;
                existingPurchase.SupplierContactNumber = obj.SupplierContactNumber;
                existingPurchase.SupplierName = obj.SupplierName;
                existingPurchase.TotalAmount = obj.TotalAmount;
                //await _purchaseRepository.Update(edititem);

                existingPurchase.purchaseItems.Clear();
                //obj.purchaseItems.ToList().ForEach(x =>x.PurchaseId =)
                //existingPurchase.purchaseItems.ToList().ForEach(x => { x.PurchaseId = obj.PurchaseId; });
                existingPurchase.purchaseItems = newPurchase;

                await _unitOfWork.SaveAsync();
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
            sell.transactionDateTime = DateTime.UtcNow;
            sell.SellItems.ToList().ForEach(x => { x.InvoiceNo = sell.InvoiceNo; });
            await _sellRepository.Add(sell);
            await _unitOfWork.SaveAsync();
            return InventoryConstants.SuccessMessage;
        }

        public async Task editSell(Sell obj)
        {
            var response = await GetSellDetailsfromDB(obj.Id ?? 0);
            if (response != null)
            {
                // Stock modification logic should be Written here
                // By comparing old and new sellItems
                var edititem = response;
                await stockModificationOnSellItemChange(edititem.SellItems.ToList() ?? new List<SellItem>(),
                                                        obj.SellItems.ToList() ?? new List<SellItem>());
                edititem.SellItems = obj.SellItems;
                edititem.Comment = obj.Comment;
                edititem.CustomerAddress = obj.CustomerAddress;
                edititem.CustomerName = obj.CustomerName;
                edititem.PhoneNumber = obj.PhoneNumber;
                edititem.SellDate = obj.SellDate;
                edititem.TotalAmount = obj.TotalAmount;
                await _sellRepository.Update(edititem);
                await _unitOfWork.SaveAsync();
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
                        var response = await _stockRepository.GetByProductId(matchedOldItem.ProductId);

                        if (response != null)
                        {
                            var matchedStockitem = response ?? new Stock();
                            matchedStockitem.Quantity = quantitydiffrence > 0 ? matchedStockitem.Quantity + quantitydiffrence : matchedStockitem.Quantity - item.Quantity;
                            await _stockRepository.Update(matchedStockitem);
                        }
                    }
                    oldItems.Remove(matchedOldItem);
                }
                else
                {
                    await _Stockservice.AddNewStock(new List<PurchaseItem> { item });
                }
            }

            foreach (var item in oldItems)
            {
                var filterParameters = new Dictionary<string, object>()
                    {
                       {nameof(Stock.ProductId),item.ProductId}
                    };
                var response = await _stockRepository.GetByProductId(item.ProductId);
                if (response != null)
                {
                    var matchedOldItem = response;
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
                        var response = await _stockRepository.GetByProductId(item.ProductId);
                        if (response != null)
                        {
                            var matchedStockitem = response ?? new Stock();
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
                var response = await _stockRepository.GetByProductId(item.ProductId);
                if (response != null)
                {
                    var matchedOldItem = response;
                    matchedOldItem.Quantity += item.Quantity;
                    await _stockRepository.Update(matchedOldItem);
                }
            }
        }

        public async Task<Sell> GetSellDetailsfromDB(long id)
        {
            return await _sellRepository.GetById(id);
        }
    }
}