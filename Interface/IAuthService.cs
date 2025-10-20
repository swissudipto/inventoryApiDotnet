using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface IAuthService
    {
        Task<User> GetByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }
}