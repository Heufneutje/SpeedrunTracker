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

    public bool HasEntities => Entities?.Any() == true && !IsRunningBackgroundTask;

    public FollowedEntityViewModel(IGameService gameService, IGameSeriesService gameSeriesService, IUserService userService, ILocalFollowService localFollowService, IToastService toastService) : base(toastService)
    {
        _gameService = gameService;
        _gameSeriesService = gameSeriesService;
        _userService = userService;
        _localFollowService = localFollowService;
        Entities = new ObservableCollection<EntityGroup>();
    }

    public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateAsync);

    private async Task NavigateAsync(Entity entity)
    {
        switch ((EntityType)entity.SearchObject)
        {
            case EntityType.Games:
                Game game = await ExecuteNetworkTask(_gameService.GetGameAsync(entity.Id));
                if (game != null)
                    await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
                break;
            case EntityType.Series:
                GameSeries series = await ExecuteNetworkTask(_gameSeriesService.GetGameSeriesAsync(entity.Id));
                if (series != null)
                    await Shell.Current.GoToAsync(Routes.SeriesDetailPageRoute, "Series", series);
                break;
            case EntityType.Users:
                User user = await ExecuteNetworkTask(_userService.GetUserAsync(entity.Id));
                if (user != null)
                    await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
                break;
        }
    }

    public async Task LoadFollowedEntities()
    {
        IsRunningBackgroundTask = true;

        List<EntityGroup> entities = new();
        List<FollowedEntity> followedEntities = await _localFollowService.GetFollowedEntitiesAsync();

        foreach (IGrouping<EntityType, FollowedEntity> grouping in followedEntities.OrderBy(x => x.Type).GroupBy(x => x.Type))
        {
            entities.Add(new EntityGroup(grouping.Key, grouping.ToList().Select(x => new Entity()
            {
                Id = x.Id,
                Title = x.Title,
                Subtitle = x.Subtitle,
                ImageUrl = x.ImageUrl,
                SearchObject = grouping.Key
            }).ToList()));
        }

        Entities = entities.AsObservableCollection();
        IsRunningBackgroundTask = false;
        NotifyPropertyChanged(nameof(Entities));
        NotifyPropertyChanged(nameof(HasEntities));
    }
}
