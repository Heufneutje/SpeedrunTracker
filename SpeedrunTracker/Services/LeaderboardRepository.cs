using Refit;
using SpeedrunTracker.Interfaces;

namespace SpeedrunTracker.Repository;

public class LeaderboardRepository : ILeaderboardRepository
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardRepository(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    public Task<BaseData<Leaderboard>> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults)
    {
        try
        {
            return _leaderboardService.GetFullGameLeaderboardAsync(gameId, categoryId, variables, maxResults);
        }
        catch (ApiException ex)
        {
            return null;
        }
    }

    public Task<BaseData<Leaderboard>> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults)
    {
        try
        {
            return _leaderboardService.GetLevelLeaderboardAsync(gameId, levelId, categoryId, variables, maxResults);
        }
        catch (ApiException ex)
        {
            return null;
        }
    }
}
