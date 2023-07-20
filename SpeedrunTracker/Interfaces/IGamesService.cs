using Refit;
using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces
{
    public interface IGamesService
    {
        [Get("/games?name={name}")]
        Task<PagedData<List<Game>>> SearchGamesAsync(string name);

        [Get("/games/{gameId}/categories")]
        Task<BaseData<List<Category>>> GetGameCategoriesAsync(string gameId);

        [Get("/games/{gameId}/levels")]
        Task<BaseData<List<Level>>> GetGameLevelsAsync(string gameId);

        [Get("/games/{gameId}/variables")]
        Task<BaseData<List<Variable>>> GetGameVariablesAsync(string gameId);
    }
}
