namespace SpeedrunTracker.Services;

public class GameSeriesService : IGameSeriesService
{
    private IGameSeriesRepository _gameSeriesRepository;

    public GameSeriesService(IGameSeriesRepository gameSeriesRepository)
    {
        _gameSeriesRepository = gameSeriesRepository;
    }

    public async Task<GameSeries> GetGameSeriesAsync(string seriesId)
    {
        return (await _gameSeriesRepository.GetGameSeriesAsync(seriesId))?.Data;
    }

    public Task<PagedData<List<Game>>> GetGameSeriesEntriesAsync(string seriesId, int offset)
    {
        return _gameSeriesRepository.GetGameSeriesEntriesAsync(seriesId, offset);
    }

    public Task<PagedData<List<GameSeries>>> SearchGameSeriesAsync(string name)
    {
        return _gameSeriesRepository.SearchGameSeriesAsync(name);
    }
}
