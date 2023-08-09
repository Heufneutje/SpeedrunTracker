namespace SpeedrunTracker.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardService;

    public LeaderboardService(ILeaderboardRepository leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    public Task<BaseData<Leaderboard>> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults)
    {
        return _leaderboardService.GetFullGameLeaderboardAsync(gameId, categoryId, variables, maxResults);
    }

    public Task<BaseData<Leaderboard>> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults)
    {
        return _leaderboardService.GetLevelLeaderboardAsync(gameId, levelId, categoryId, variables, maxResults);
    }
}
