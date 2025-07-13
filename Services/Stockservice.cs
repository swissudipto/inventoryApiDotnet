using inventoryApiDotnet.Constants;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Services
{
  public class Stockservice : IStockservice
  {
    public readonly IStockRepository _stockRepository;
    public readonly IProductService _productsrvice;
    public Stockservice(IStockRepository stockRepository,
                        IProductService productservice)
    {
      _stockRepository = stockRepository;
      _productsrvice = productservice;
    }

    public async Task<string> AddNewStock(PurchaseItem obj)
    {

      var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Stock.ProductId),obj.ProductId}
              };

      var response = await _stockRepository
                    .QueryCollectionAsync(new Stock(), filterParameters);
      if (response.Count == 0)
      {
        var product = await _productsrvice.GetProductById((long)obj.ProductId);
        var newStock = new Stock
        {
          ProductId = obj.ProductId > 0 ? obj.ProductId : 0,
          ProductName = product.ProductName != null ? product.ProductName : "",
          Quantity = obj.Quantity > 0 ? obj.Quantity : 0,
          AvarageBuyingPrice = Math.Round((double)obj.Amount / obj.Quantity, 2)
        };
        await _stockRepository.Add(newStock);
      }
      else
      {
        var existingstock = response.FirstOrDefault();
        var newAvarageBuyingprice = Math.Round((double)obj.Amount / obj.Quantity, 2);
        existingstock.Quantity += obj.Quantity;
        existingstock.AvarageBuyingPrice = Math.Round((double)(existingstock.AvarageBuyingPrice + newAvarageBuyingprice) / 2, 2);
        await _stockRepository.Update(existingstock);
      }
      return "success";
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
      var filterParameters = new Dictionary<string, object>()
              {
                {nameof(sellItem.ProductId),sellItem.ProductId}
              };

      var response = _stockRepository
                    .QueryCollectionAsync(new Stock(), filterParameters);

      if (response?.Result?.FirstOrDefault()?.Quantity.Value < sellItem.Quantity)
      {
        message = InventoryConstants.OutofStockErrorMessage
                  + sellItem.ProductName
                  + " : "
                  + response.Result?.FirstOrDefault().Quantity.Value;
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
                      .QueryCollectionAsync(new Stock(), filterParameters);

      var ProductStock = response.FirstOrDefault();
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

      var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);

      if (response == null)
      {
        throw new Exception($"No stock found for ProductId: {productId}");
      }

      return response.FirstOrDefault() ?? new Stock();
    }
  }
}