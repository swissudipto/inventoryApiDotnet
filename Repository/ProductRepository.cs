using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;

namespace inventoryApiDotnet.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }
    }
}
