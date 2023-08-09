using Refit;

namespace SpeedrunTracker.Services;

public interface ILeaderboardRepository
{
    [Get("/leaderboards/{gameId}/category/{categoryId}?embed=players&top={maxResults}{variables}")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<BaseData<Leaderboard>> GetFullGameLeaderboardAsync(string gameId, string categoryId, string variables, int maxResults);

    [Get("/leaderboards/{gameId}/level/{levelId}/{categoryId}?embed=players&top={maxResults}{variables}")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<BaseData<Leaderboard>> GetLevelLeaderboardAsync(string gameId, string levelId, string categoryId, string variables, int maxResults);
}
