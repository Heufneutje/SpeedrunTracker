namespace SpeedrunTracker.Interfaces;

public interface ILeaderboardRepository
{
    Task<BaseData<Leaderboard>> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults);

    Task<BaseData<Leaderboard>> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults);
}
