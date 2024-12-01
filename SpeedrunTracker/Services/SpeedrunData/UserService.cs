namespace SpeedrunTracker.Services.SpeedrunData;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        return (await _userRepository.GetUserAsync(userId))?.Data;
    }

    public async Task<List<LeaderboardEntry>> GetUserPersonalBestsAsync(string userId)
    {
        return (await _userRepository.GetUserPersonalBestsAsync(userId))?.Data ?? [];
    }

    public async Task<User?> GetUserProfileAsync()
    {
        return (await _userRepository.GetUserProfileAsync(await SecureStorage.GetAsync(Constants.ApiKey)))?.Data;
    }

    public Task<PagedData<List<User>>> SearchUsersAsync(string name)
    {
        return _userRepository.SearchUsersAsync(name);
    }
}
