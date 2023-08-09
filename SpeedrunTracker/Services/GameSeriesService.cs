namespace SpeedrunTracker.Services;

public class GameSeriesService : IGameSeriesService
{
    private IGameSeriesRepository _gamesSeriesService;

    public GameSeriesService(IGameSeriesRepository gamesSeriesService)
    {
        _gamesSeriesService = gamesSeriesService;
    }

    public Task<BaseData<GameSeries>> GetGameSeriesAsync(string seriesId)
    {
        return _gamesSeriesService.GetGameSeriesAsync(seriesId);
    }

    public Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset)
    {
        return _gamesSeriesService.GetGameSeriesEntriesAsync(seriesId, offset);
    }

    public Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name)
    {
        return _gamesSeriesService.SearchGameSeriesAsync(name);
    }
}
