namespace SpeedrunTracker.Services;

public interface IGameSeriesService
{
    Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name);

    Task<GameSeries> GetGameSeriesAsync(string seriesId);

    Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset);
}
