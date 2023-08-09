namespace SpeedrunTracker.Services;

public class GameSeriesService : IGameSeriesService
{
    private IGameSeriesRepository _gamesSeriesRepository;

    public GameSeriesService(IGameSeriesRepository gamesSeriesRepository)
    {
        _gamesSeriesRepository = gamesSeriesRepository;
    }

    public async Task<GameSeries> GetGameSeriesAsync(string seriesId)
    {
        return (await _gamesSeriesRepository.GetGameSeriesAsync(seriesId))?.Data;
    }

    public Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset)
    {
        return _gamesSeriesRepository.GetGameSeriesEntriesAsync(seriesId, offset);
    }

    public Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name)
    {
        return _gamesSeriesRepository.SearchGameSeriesAsync(name);
    }
}
