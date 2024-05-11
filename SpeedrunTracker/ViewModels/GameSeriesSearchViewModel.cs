using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class GameSeriesSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IGameSeriesService _gameSeriesService;

    public GameSeriesSearchViewModel(IGameSeriesService gameSeriesService, IToastService toastService) : base(toastService)
    {
        _gameSeriesService = gameSeriesService;
    }

    public override string SearchTextPlaceholder => "Search for game series...";

    protected override async Task NavigateToAsync(Entity entity)
    {
        if (entity.SearchObject is GameSeries series)
            await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<GameSeries>> apiData = await ExecuteNetworkTask(_gameSeriesService.SearchGameSeriesAsync(Query.Trim()));
        if (apiData == null)
            return null;

        return apiData.Data.Select(x => new Entity()
        {
            Title = x.Names.International,
            Subtitle = $"Created: {x.Created?.ToString("yyyy-MM-dd") ?? "Unknown"}",
            ImageUrl = x.Assets.CoverSmall.Uri,
            SearchObject = x
        }).ToList();
    }
}
