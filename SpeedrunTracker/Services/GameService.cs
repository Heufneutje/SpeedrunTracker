namespace SpeedrunTracker.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<Game> GetGameAsync(string gameId)
    {
        return (await _gameRepository.GetGameAsync(gameId))?.Data;
    }

    public async Task<List<Category>> GetGameCategoriesAsync(string gameId)
    {
        return (await _gameRepository.GetGameCategoriesAsync(gameId))?.Data;
    }

    public async Task<List<Level>> GetGameLevelsAsync(string gameId)
    {
        return (await _gameRepository.GetGameLevelsAsync(gameId))?.Data;
    }

    public async Task<List<Variable>> GetGameVariablesAsync(string gameId)
    {
        return (await _gameRepository.GetGameVariablesAsync(gameId))?.Data;
    }

    public Task<PagedData<List<Game>>> SearchGamesAsync(string name)
    {
        return _gameRepository.SearchGamesAsync(name);
    }
}
