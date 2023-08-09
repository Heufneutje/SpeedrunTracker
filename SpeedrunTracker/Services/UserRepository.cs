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

    public Task<BaseData<List<LeaderboardEntry>>> GetUserPersonalBestsAsync(string userId)
    {
        return _userService.GetUserPersonalBestsAsync(userId);
    }

    public async Task<BaseData<User>> GetUserProfileAsync()
    {
        return await _userService.GetUserProfileAsync(await SecureStorage.GetAsync(Constants.ApiKey));
    }

    public Task<PagedData<List<User>>> SearchUsersAsync(string name)
    {
        return _userService.SearchUsersAsync(name);
    }
}
