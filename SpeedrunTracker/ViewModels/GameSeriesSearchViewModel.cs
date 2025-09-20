using CommunityToolkit.Maui.Core;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class GameSeriesSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IGameSeriesService _gameSeriesService;

    public GameSeriesSearchViewModel(
        IGameSeriesService gameSeriesService,
        IToastService toastService,
        IPopupService popupService
    )
        : base(toastService, popupService)
    {
        _gameSeriesService = gameSeriesService;
    }

    public override string SearchTextPlaceholder => "Search for game series...";

    protected override async Task NavigateToAsync()
    {
        if (SelectedEntity?.SearchObject is GameSeries series)
        {
            ShowActivityIndicator();
            await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
            SelectedEntity = null;
        }
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<GameSeries>>? apiData = await ExecuteNetworkTask(
            _gameSeriesService.SearchGameSeriesAsync(Query?.Trim() ?? string.Empty)
        );
        if (apiData is null)
            return [];

        return apiData
            .Data.Select(x => new Entity()
            {
                Title = x.Names.International,
                Subtitle = $"Created: {x.Created?.ToString("yyyy-MM-dd") ?? "Unknown"}",
                ImageUrl = x.Assets?.CoverSmall?.SecureUri,
                SearchObject = x,
            })
            .ToList();
    }
}
