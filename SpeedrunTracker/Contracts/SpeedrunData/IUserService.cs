namespace SpeedrunTracker.Contracts.SpeedrunData;

public interface IUserService
{
    Task<User> GetUserAsync(string userId);

    Task<PagedData<List<User>>> SearchUsersAsync(string name);

    Task<List<LeaderboardEntry>> GetUserPersonalBestsAsync(string userId);

    Task<User> GetUserProfileAsync();
}
