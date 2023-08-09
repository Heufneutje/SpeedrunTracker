namespace SpeedrunTracker.Services;

public interface IGamesService
{
    Task<PagedData<List<Game>>> SearchGamesAsync(string name);

    Task<Game> GetGameAsync(string gameId);

    Task<List<Category>> GetGameCategoriesAsync(string gameId);

    Task<List<Level>> GetGameLevelsAsync(string gameId);

    Task<List<Variable>> GetGameVariablesAsync(string gameId);
}
