using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces;

public interface IGamesRepository
{
    Task<PagedData<List<Game>>> SearchGamesAsync(string name);
    Task<BaseData<List<Category>>> GetGameCategoriesAsync(string gameId);
    Task<BaseData<List<Level>>> GetGameLevelsAsync(string gameId);
    Task<BaseData<List<Variable>>> GetGameVariablesAsync(string gameId);
}
