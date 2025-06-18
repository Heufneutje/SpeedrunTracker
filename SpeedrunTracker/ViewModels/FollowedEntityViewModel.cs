using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public partial class FollowedEntityViewModel : BaseNetworkActionViewModel
{
    private readonly IGameService _gameService;
    private readonly IGameSeriesService _gameSeriesService;
    private readonly IUserService _userService;
    private readonly ILocalFollowService _localFollowService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEntities))]
    private List<EntityGroup> _entities;

    public bool HasEntities => Entities?.Count > 0;

    public FollowedEntityViewModel(
        IGameService gameService,
        IGameSeriesService gameSeriesService,
        IUserService userService,
        ILocalFollowService localFollowService,
        IToastService toastService,
        IPopupService popupService
    )
        : base(toastService, popupService)
    {
        _gameService = gameService;
        _gameSeriesService = gameSeriesService;
        _userService = userService;
        _localFollowService = localFollowService;
        Entities = [];
    }

    [RelayCommand]
    private async Task NavigateAsync(Entity? entity)
    {
        if (entity is null)
            return;

        await ShowActivityIndicatorAsync();
        switch ((EntityType?)entity.SearchObject)
        {
            case EntityType.Games:
                Game? game = await ExecuteNetworkTask(_gameService.GetGameAsync(entity.Id));
                if (game is not null)
                {
                    await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
                    return;
                }
                break;

            case EntityType.Series:
                GameSeries? series = await ExecuteNetworkTask(_gameSeriesService.GetGameSeriesAsync(entity.Id));
                if (series is not null)
                {
                    await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
                    return;
                }
                break;

            case EntityType.Users:
                User? user = await ExecuteNetworkTask(_userService.GetUserAsync(entity.Id));
                if (user is not null)
                {
                    await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
                    return;
                }
                break;
        }

        await CloseActivityIndicatorAsync();
    }

    public async Task LoadFollowedEntities()
    {
        List<EntityGroup> entities = [];
        List<FollowedEntity> followedEntities = await _localFollowService.GetFollowedEntitiesAsync();

        foreach (
            IGrouping<EntityType, FollowedEntity> grouping in followedEntities.OrderBy(x => x.Type).GroupBy(x => x.Type)
        )
        {
            entities.Add(
                new EntityGroup(
                    grouping.Key,
                    grouping
                        .Select(x => new Entity()
                        {
                            Id = x.Id ?? string.Empty,
                            Title = x.Title,
                            Subtitle = x.Subtitle,
                            ImageUrl = x.ImageUrl,
                            SearchObject = grouping.Key,
                        })
                        .ToList()
                )
            );
        }

        Entities = entities;
    }
}
