namespace SpeedrunTracker.Services;

public interface ILeaderboardService
{
    Task<Leaderboard> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults);

    Task<Leaderboard> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults);
}
