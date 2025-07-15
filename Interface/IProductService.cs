using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IProductService
    {
        Task<Product> SaveProduct(Product obj);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(long productID);
        Task<bool> IsProductNameExists(string productName);
    }
}