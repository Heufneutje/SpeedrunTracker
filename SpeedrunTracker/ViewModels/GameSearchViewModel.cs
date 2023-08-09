using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;

namespace SpeedrunTracker.ViewModels;

public class GameSearchViewModel : BaseSearchEntityViewModel
{
    private IGamesService _gamesService;

    public override string SearchTextPlaceholder => "Search for games...";

    public GameSearchViewModel(IToastService toastService, IGamesService gamesService) : base(toastService)
    {
        _gamesService = gamesService;
    }

    protected override async Task NavigateToAsync(Entity entity)
    {
        if (entity.SearchObject is Game game)
            await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<Game>> apiData = await ExecuteNetworkTask(_gamesService.SearchGamesAsync(Query.Trim()));
        if (apiData == null)
            return null;

        return apiData.Data.OrderBy(x => x.IsRomhack).Select(x => new Entity()
        {
            Title = x.Names.International,
            Subtitle = $"Released: {x.Released}",
            ImageUrl = x.Assets.CoverSmall.FixedGameAssetUri,
            SearchObject = x
        }).ToList();
    }
}
