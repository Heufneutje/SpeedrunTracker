namespace SpeedrunTracker.Interfaces;

public interface IUserRepository
{
    Task<BaseData<User>> GetUserAsync(string userId);

    Task<PagedData<List<User>>> SearchUsersAsync(string name);

    Task<BaseData<List<LeaderboardEntry>>> GetUserPersonalBestsAsync(string userId);
}
