using CommunityToolkit.Maui.Core;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class GameSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IGameService _gameService;

    public override string SearchTextPlaceholder => "Search for games...";

    public GameSearchViewModel(IToastService toastService, IGameService gameService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _gameService = gameService;
    }

    protected override async Task NavigateToAsync(Entity? entity)
    {
        if (entity?.SearchObject is Game game)
        {
            ShowActivityIndicator();
            await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
        }
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<Game>>? apiData = await ExecuteNetworkTask(_gameService.SearchGamesAsync(Query.Trim()));
        if (apiData == null)
            return [];

        return apiData
            .Data.OrderBy(x => x.IsRomhack)
            .Select(x => new Entity()
            {
                Title = x.Names.International,
                Subtitle = $"Released: {x.Released}",
                ImageUrl = x.Assets?.CoverSmall?.Uri,
                SearchObject = x,
            })
            .ToList();
    }
}
