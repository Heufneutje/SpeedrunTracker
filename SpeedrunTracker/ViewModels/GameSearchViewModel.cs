using CommunityToolkit.Maui.Core;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public class GameSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IGameService _gameService;

    public override string SearchTextPlaceholder => AppStrings.GameSearchPagePlaceholderText;

    public GameSearchViewModel(IToastService toastService, IGameService gameService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _gameService = gameService;
    }

    protected override async Task NavigateToAsync()
    {
        if (SelectedEntity?.SearchObject is Game game)
        {
            ShowActivityIndicator();
            await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
            SelectedEntity = null;
        }
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<Game>>? apiData = await ExecuteNetworkTask(_gameService.SearchGamesAsync(Query?.Trim() ?? string.Empty));
        if (apiData is null)
            return [];

        return apiData
            .Data.OrderBy(x => x.IsRomhack)
            .Select(x => new Entity()
            {
                Title = x.Names.International,
                Subtitle = $"{AppStrings.EntitySubtitleReleased}: {x.Released}",
                ImageUrl = x.Assets?.CoverSmall?.SecureUri,
                SearchObject = x,
            })
            .ToList();
    }
}
