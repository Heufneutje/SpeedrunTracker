using Refit;

namespace SpeedrunTracker.Interfaces
{
    public interface IGamesService
    {
        [Get("/games?name={name}&embed=platforms,moderators")]
        Task<PagedData<List<Game>>> SearchGamesAsync(string name);

        [Get("/games/{gameId}?embed=platforms,moderators")]
        Task<BaseData<Game>> GetGameAsync(string gameId);

        [Get("/games/{gameId}/categories")]
        Task<BaseData<List<Category>>> GetGameCategoriesAsync(string gameId);

        [Get("/games/{gameId}/levels")]
        Task<BaseData<List<Level>>> GetGameLevelsAsync(string gameId);

        [Get("/games/{gameId}/variables")]
        Task<BaseData<List<Variable>>> GetGameVariablesAsync(string gameId);
    }
}
