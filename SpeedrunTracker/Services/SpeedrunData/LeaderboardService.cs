namespace SpeedrunTracker.Services.SpeedrunData;

public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardService;

    public LeaderboardService(ILeaderboardRepository leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    public async Task<Leaderboard?> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults)
    {
        return (await _leaderboardService.GetFullGameLeaderboardAsync(gameId, categoryId, variables, maxResults))?.Data;
    }

    public async Task<Leaderboard?> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults)
    {
        return (await _leaderboardService.GetLevelLeaderboardAsync(gameId, levelId, categoryId, variables, maxResults))?.Data;
    }
}
