using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;

namespace inventoryApiDotnet.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
