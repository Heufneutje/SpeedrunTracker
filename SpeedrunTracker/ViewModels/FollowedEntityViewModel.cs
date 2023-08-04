using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class FollowedEntityViewModel : BaseViewModel
{
    private readonly IGamesRepository _gamesRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILocalFollowService _localFollowService;

    public ObservableCollection<EntityGroup> Entities { get; set; }

    public bool HasEntities => Entities?.Any() == true && !IsRunningBackgroundTask;

    public FollowedEntityViewModel(IGamesRepository gamesRepository, IUserRepository userRepository, ILocalFollowService localFollowService)
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
                Game game = (await _gamesRepository.GetGameAsync(entity.Id)).Data;
                await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
                break;
            case EntityType.Users:
                User user = (await _userRepository.GetUserAsync(entity.Id)).Data;
                await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
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
