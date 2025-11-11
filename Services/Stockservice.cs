using inventoryApiDotnet.Constants;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Services
{
  public class Stockservice : IStockservice
  {
    private readonly IStockRepository _stockRepository;
    private readonly IProductService _productsrvice;
    private readonly IUnitOfWork _unitOfWork;

    public Stockservice(IStockRepository stockRepository,
                        IProductService productservice,
                        IUnitOfWork unitOfWork)
    {
      _stockRepository = stockRepository;
      _productsrvice = productservice;
      _unitOfWork = unitOfWork;
    }

    public async Task AddNewStock(List<PurchaseItem> obj)
    {
      foreach (var item in obj)
      {
        var response = await _stockRepository
              .GetByProductId(item.ProductId);

        if (response is null)
        {
          var product = await _productsrvice.GetProductById((long)item.ProductId);
          var newStock = new Stock
          {
            ProductId = item.ProductId > 0 ? item.ProductId : 0,
            ProductName = product.ProductName != null ? product.ProductName : "",
            Quantity = item.Quantity > 0 ? item.Quantity : 0,
            AvarageBuyingPrice = Math.Round((double)item.Amount / item.Quantity, 2)
          };
          await _stockRepository.Add(newStock);
        }
        else
        {
          var existingstock = response;
          var newAvarageBuyingprice = Math.Round((double)item.Amount / item.Quantity, 2);
          existingstock.Quantity += item.Quantity;
          existingstock.AvarageBuyingPrice = Math.Round((double)(existingstock.AvarageBuyingPrice + newAvarageBuyingprice) / 2, 2);
          await _stockRepository.Update(existingstock);
        }
      }
    }

    public async Task<List<Stock>> GetAllStock()
    {
      var response = await _stockRepository.GetAll();
      return response.Where(y => y.Quantity > 0)
                     .OrderByDescending(x => x.Quantity).ToList();
    }

    public async Task<List<Stock>> GetAllStock(int page, int pageSize)
    {
      var response = await _stockRepository.GetAllbyPage(page, pageSize);
      return response.Where(y => y.Quantity > 0)
                     .OrderByDescending(x => x.Quantity).ToList();
    }

    public Boolean checkIfProductInStock(SellItem sellItem, out string message)  // Need To test this Method
    {
      var response = _stockRepository
                    .GetByProductId(sellItem.ProductId);

      if (response is null || response?.Result?.Quantity.Value < sellItem.Quantity)
      {
        message = InventoryConstants.OutofStockErrorMessage
                  + sellItem.ProductName
                  + " : "
                  + response.Result?.Quantity.Value;
        return false;
      }
      message = InventoryConstants.SuccessMessage;
      return true;
    }

    public async Task afterSellStockModification(SellItem sellItem)
    {
      var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Stock.ProductId),sellItem.ProductId}
              };

      var response = await _stockRepository
                    .GetByProductId(sellItem.ProductId);

      var ProductStock = response;
      ProductStock.Quantity -= sellItem.Quantity;
      ProductStock.AvarageBuyingPrice = ProductStock.Quantity == 0 ? 0 : ProductStock.AvarageBuyingPrice;
      await _stockRepository.Update(ProductStock);
    }

    public async Task<Stock> GetStockByProductId(long productId)
    {
      var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Stock.ProductId),productId}
              };

      var response = await _stockRepository
              .GetByProductId(productId);

      if (response == null)
      {
        throw new Exception($"No stock found for ProductId: {productId}");
      }

      return response ?? new Stock();
    }
  }
}