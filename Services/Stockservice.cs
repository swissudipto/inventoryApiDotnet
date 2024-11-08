using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace inventoryApiDotnet.Services
{
    public class Stockservice : IStockservice
    {
        public readonly IStockRepository _stockRepository;
        public Stockservice(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
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
                var newStock = new Stock
                {
                    ProductId = obj.ProductId > 0 ? obj.ProductId : 0,
                    Quantity = obj.Quantity > 0 ? obj.Quantity : 0
                };
                await _stockRepository.Add(newStock);
            }
            else{
                var existingstock = response.FirstOrDefault();
                existingstock.Quantity +=obj.Quantity; 
                await _stockRepository.Update(existingstock);
            }
            return "success";
        }
    }
}