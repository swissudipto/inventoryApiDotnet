using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace inventoryApiDotnet.Services
{
    public class Stockservice : IStockservice
    {
        public readonly IStockRepository _stockRepository;
        public readonly IProductService _productsrvice;
        public Stockservice(IStockRepository stockRepository, IProductService productservice)
        {
            _stockRepository = stockRepository;
            _productsrvice = productservice;
        }

        public async Task<string> AddNewStock(Purchase obj)
        {

            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Stock.ProductId),obj.ProductId}
              };

            var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);
            if (response.Count == 0)
            {
                var product = await _productsrvice.GetProductById((long)obj.ProductId);
                var newStock = new Stock
                {
                    ProductId = obj.ProductId > 0 ? obj.ProductId : 0,
                    ProductName = product.ProductName != null ? product.ProductName : "",
                    Quantity = obj.Quantity > 0 ? obj.Quantity : 0
                };
                await _stockRepository.Add(newStock);
            }
            else
            {
                var existingstock = response.FirstOrDefault();
                existingstock.Quantity += obj.Quantity;
                await _stockRepository.Update(existingstock);
            }
            return "success";
        }

        public async Task<List<Stock>> GetAllStock()
        {
            var response = await _stockRepository.GetAll();
            return response.OrderByDescending(x => x.Quantity).ToList();
        }

        public Boolean checkIfProductInStock(Sell sell, out string message)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(sell.ProductId),sell.ProductId}
              };

            var response = _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);

            if (response?.Result?.FirstOrDefault()?.Quantity.Value < sell.Quantity)
            {
                message = "Not Enough Stock. Available Quantity of " + sell.ProductName + " : " + response.Result?.FirstOrDefault().Quantity.Value;
                return false;
            }
            message = "Success";
            return true;
        }

        public async Task afterSellStockModification(Sell sell)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Stock.ProductId),sell.ProductId}
              };

            var response = await _stockRepository.QueryCollectionAsync(new Stock(), filterParameters);
            var ProductStock = response.FirstOrDefault();
            ProductStock.Quantity -= sell.Quantity;
            await _stockRepository.Update(ProductStock);
        }
    }
}