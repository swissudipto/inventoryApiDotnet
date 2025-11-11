using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;


namespace inventoryApiDotnet.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
