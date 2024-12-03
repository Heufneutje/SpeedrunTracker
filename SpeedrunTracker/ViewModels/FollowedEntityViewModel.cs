using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class FollowedEntityViewModel : BaseNetworkActionViewModel
{
    private readonly IGameService _gameService;
    private readonly IGameSeriesService _gameSeriesService;
    private readonly IUserService _userService;
    private readonly ILocalFollowService _localFollowService;

    public ObservableCollection<EntityGroup> Entities { get; set; }

    public bool HasEntities => Entities?.Any() == true;

    public FollowedEntityViewModel(IGameService gameService, IGameSeriesService gameSeriesService, IUserService userService, ILocalFollowService localFollowService, IToastService toastService, IPopupService popupService) : base(toastService, popupService)
    {
        _gameService = gameService;
        _gameSeriesService = gameSeriesService;
        _userService = userService;
        _localFollowService = localFollowService;
        Entities = [];
    }

    public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateAsync);

    private async Task NavigateAsync(Entity? entity)
    {
        if (entity == null)
            return;

        ShowActivityIndicator();
        switch ((EntityType?)entity.SearchObject)
        {
            case EntityType.Games:
                Game? game = await ExecuteNetworkTask(_gameService.GetGameAsync(entity.Id));
                if (game != null)
                {
                    await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
                    return;
                }
                break;
            case EntityType.Series:
                GameSeries? series = await ExecuteNetworkTask(_gameSeriesService.GetGameSeriesAsync(entity.Id));
                if (series != null)
                {
                    await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
                    return;
                }
                break;
            case EntityType.Users:
                User? user = await ExecuteNetworkTask(_userService.GetUserAsync(entity.Id));
                if (user != null)
                {
                    await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
                    return;
                }
                break;
        }

        CloseActivityIndicator();
    }

    public async Task LoadFollowedEntities()
    {
        List<EntityGroup> entities = [];
        List<FollowedEntity> followedEntities = await _localFollowService.GetFollowedEntitiesAsync();

        foreach (IGrouping<EntityType, FollowedEntity> grouping in followedEntities.OrderBy(x => x.Type).GroupBy(x => x.Type))
        {
            entities.Add(new EntityGroup(grouping.Key, grouping.Select(x => new Entity()
            {
                Id = x.Id ?? string.Empty,
                Title = x.Title,
                Subtitle = x.Subtitle,
                ImageUrl = x.ImageUrl,
                SearchObject = grouping.Key
            }).ToList()));
        }

        Entities = entities.AsObservableCollection();
        NotifyPropertyChanged(nameof(Entities));
        NotifyPropertyChanged(nameof(HasEntities));
    }
}
