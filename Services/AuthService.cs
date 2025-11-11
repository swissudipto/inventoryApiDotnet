using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IUnitOfWork _unitOfWork;

  public AuthService(IUserRepository userRepository,
                    IUnitOfWork unitOfWork)
  {
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<User> GetByUsernameAsync(string username)
  {
    var filterParameters = new Dictionary<string, object>()
                        {
                            {nameof(User.Username),username}
                        };
    var response = await _userRepository.QueryCollectionAsync(new User(), filterParameters);
    return response.FirstOrDefault() ?? null;
  }

  public async Task CreateUserAsync(User user)
  {
    await _userRepository.Add(user);
    await _unitOfWork.SaveAsync();
  }
}
