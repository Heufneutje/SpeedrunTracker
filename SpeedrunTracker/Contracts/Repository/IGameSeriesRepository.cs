using Refit;

namespace SpeedrunTracker.Contracts.Repository;

public interface IGameSeriesRepository
{
    [Get("/series?name={name}")]
    Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name);

    [Get("/series/{seriesId}")]
    Task<BaseData<GameSeries>?> GetGameSeriesAsync(string seriesId);

    [Get("/series/{seriesId}/games?embed=platforms,moderators&offset={offset}")]
    Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset);
}
