namespace SpeedrunTracker.Services;

public class GamesService : IGameService
{
    private readonly IGameRepository _gamesRepository;

    public GamesService(IGameRepository gamesRepository)
    {
        _gamesRepository = gamesRepository;
    }

    public async Task<Game> GetGameAsync(string gameId)
    {
        return (await _gamesRepository.GetGameAsync(gameId))?.Data;
    }

    public async Task<List<Category>> GetGameCategoriesAsync(string gameId)
    {
        return (await _gamesRepository.GetGameCategoriesAsync(gameId))?.Data;
    }

    public async Task<List<Level>> GetGameLevelsAsync(string gameId)
    {
        return (await _gamesRepository.GetGameLevelsAsync(gameId))?.Data;
    }

    public async Task<List<Variable>> GetGameVariablesAsync(string gameId)
    {
        return (await _gamesRepository.GetGameVariablesAsync(gameId))?.Data;
    }

    public Task<PagedData<List<Game>>> SearchGamesAsync(string name)
    {
        return _gamesRepository.SearchGamesAsync(name);
    }
}
