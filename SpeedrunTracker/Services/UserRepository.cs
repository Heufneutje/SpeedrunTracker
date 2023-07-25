using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;

namespace SpeedrunTracker.Services;

public class UserRepository : IUserRepository
{
    private readonly IUserService _userService;

    public UserRepository(IUserService userService)
    {
        _userService = userService;
    }

    public Task<BaseData<User>> GetUserAsync(string userId)
    {
        return _userService.GetUserAsync(userId);
    }
}
