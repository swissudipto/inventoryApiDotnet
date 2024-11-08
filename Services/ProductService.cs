using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace inventoryApiDotnet.Services
{
    public class ProductService : IProductService
    {
        public readonly MongoDBService _mongoDBService;
        public readonly IProductRepository _productRepository;

        public ProductService(MongoDBService mongoDBService, IProductRepository productRepository)
        {
            _mongoDBService = mongoDBService;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product> SaveProduct(Product obj)
        {
            obj.Id = ObjectId.GenerateNewId().ToString();
            obj.ProductId = _productRepository.GetCollectionCount() + 1;
            await _productRepository.Add(obj);
            return obj;
        }

        public async Task<Product?> GetProductById(long productID)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Product.ProductId),productID}
              };
            var response = await _productRepository.QueryCollectionAsync(new Product(), filterParameters);
            return response != null ? response.FirstOrDefault() : null;
        }
    }
}