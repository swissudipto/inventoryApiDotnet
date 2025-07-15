using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using MongoDB.Bson;

namespace inventoryApiDotnet.Services
{
    public class ProductService : IProductService
    {
        public readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var result = await _productRepository.GetAll();
            return result.Where(t => t.ProductName != "");
        }

        public async Task<Product> SaveProduct(Product obj)
        {
            obj.Id = ObjectId.GenerateNewId().ToString();
            obj.ProductId = await _productRepository.GetCollectionCount() + 1;
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

        public async Task<bool> IsProductNameExists(string? productName)
        {
            var filterParameters = new Dictionary<string, object>()
              {
                {nameof(Product.ProductName),productName.Trim()}
              };
            var response = await _productRepository.QueryCollectionAsync(new Product(), filterParameters);
            return response.Count() == 0 ? false : true;
        }
    }
}