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
            return response.OrderByDescending(x=>x.Quantity).ToList();
        }
    }
}