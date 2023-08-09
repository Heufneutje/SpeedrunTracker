using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;

namespace SpeedrunTracker.ViewModels;

public class GameSeriesSearchViewModel : BaseSearchEntityViewModel
{
    private IGameSeriesRepository _gameSeriesRepository;

    public GameSeriesSearchViewModel(IGameSeriesRepository gameSeriesRepository, IToastService toastService) : base(toastService)
    {
        _gameSeriesRepository = gameSeriesRepository;
    }

    public override string SearchTextPlaceholder => "Search for game series...";

    protected override async Task NavigateToAsync(Entity entity)
    {
        if (entity.SearchObject is GameSeries series)
            await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<GameSeries>> apiData = await ExecuteNetworkTask(_gameSeriesRepository.SearchGameSeriesAsync(Query.Trim()));
        if (apiData == null)
            return null;

        return apiData.Data.Select(x => new Entity()
        {
            Title = x.Names.International,
            Subtitle = $"Created: {x.Created?.ToString("yyyy-MM-dd") ?? "Unknown"}",
            ImageUrl = x.Assets.CoverSmall.FixedGameAssetUri,
            SearchObject = x
        }).ToList();
    }
}
