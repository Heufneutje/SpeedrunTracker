using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class GameSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IGameService _gameService;

    public override string SearchTextPlaceholder => "Search for games...";

    public GameSearchViewModel(IToastService toastService, IGameService gameService) : base(toastService)
    {
        _gameService = gameService;
    }

    protected override async Task NavigateToAsync(Entity entity)
    {
        if (entity.SearchObject is Game game)
            await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<Game>> apiData = await ExecuteNetworkTask(_gameService.SearchGamesAsync(Query.Trim()));
        if (apiData == null)
            return null;

        return apiData.Data.OrderBy(x => x.IsRomhack).Select(x => new Entity()
        {
            Title = x.Names.International,
            Subtitle = $"Released: {x.Released}",
            ImageUrl = x.Assets.CoverSmall.Uri,
            SearchObject = x
        }).ToList();
    }
}
