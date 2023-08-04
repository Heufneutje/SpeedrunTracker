using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class FollowedEntityViewModel : BaseNetworkActionViewModel
{
    private readonly IGamesRepository _gamesRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILocalFollowService _localFollowService;

    public ObservableCollection<EntityGroup> Entities { get; set; }

    public bool HasEntities => Entities?.Any() == true && !IsRunningBackgroundTask;

    public FollowedEntityViewModel(IGamesRepository gamesRepository, IUserRepository userRepository, ILocalFollowService localFollowService, IToastService toastService) : base(toastService)
    {
        _gamesRepository = gamesRepository;
        _userRepository = userRepository;
        _localFollowService = localFollowService;
        Entities = new ObservableCollection<EntityGroup>();
    }

    public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateAsync);

    private async Task NavigateAsync(Entity entity)
    {
        switch ((EntityType)entity.SearchObject)
        {
            case EntityType.Games:
                BaseData<Game> game = await ExecuteNetworkTask(_gamesRepository.GetGameAsync(entity.Id));
                if (game != null)
                    await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game.Data);
                break;
            case EntityType.Users:
                BaseData<User> user = await ExecuteNetworkTask(_userRepository.GetUserAsync(entity.Id));
                if (user != null)
                    await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user.Data);
                break;
        }
    }

    public async Task LoadFollowedEntities()
    {
        IsRunningBackgroundTask = true;

        List<EntityGroup> entities = new List<EntityGroup>();
        List<FollowedEntity> followedEntities = await _localFollowService.GetFollowedEntitiesAsync();

        foreach (IGrouping<EntityType, FollowedEntity> grouping in followedEntities.GroupBy(x => x.Type))
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
