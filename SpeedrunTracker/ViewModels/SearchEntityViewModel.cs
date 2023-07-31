using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public class SearchEntityViewModel : BaseViewModel
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUserRepository _userRepository;
        private readonly SettingsViewModel _settingsViewModel;

        public ICommand SearchCommand => new AsyncRelayCommand<string>(Search, CanSearch);
        public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateTo);

        public SearchEntityViewModel(IGamesRepository gamesRepository, IUserRepository userRepository, SettingsViewModel settingsViewModel)
        {
            _gamesRepository = gamesRepository;
            _userRepository = userRepository;
            _settingsViewModel = settingsViewModel;
        }

        private ObservableCollection<EntityGroup> _entities;

        public ObservableCollection<EntityGroup> Entities
        {
            get => _entities;
            set
            {
                _entities = value;
                NotifyPropertyChanged();
            }
        }

        public async Task Search(string query)
        {
            IsRunningBackgroundTask = true;

            ObservableCollection<EntityGroup> result = new ObservableCollection<EntityGroup>();

            if (_settingsViewModel.EnableGameSearch)
            {
                IEnumerable<Game> games = (await _gamesRepository.SearchGamesAsync(query.Trim())).Data.OrderBy(x => x.IsRomhack);
                if (games.Any())
                {
                    EntityGroup gamesGroup = new EntityGroup(Model.Enum.EntityType.Games, games.Select(x => new Entity()
                    {
                        Title = x.Names.International,
                        Subtitle = $"Released: {x.Released}",
                        ImageUrl = x.Assets.CoverSmall.FixedGameAssetUri,
                        SearchObject = x
                    }).ToList());
                    result.Add(gamesGroup);
                }
            }

            if (_settingsViewModel.EnableUserSearch)
            {
                IEnumerable<User> users = (await _userRepository.SearchUsersAsync(query.Trim())).Data;
                if (users.Any())
                {
                    EntityGroup usersGroup = new EntityGroup(Model.Enum.EntityType.Users, users.Select(x => new Entity()
                    {
                        Title = x.Names.International,
                        Subtitle = $"Registered: {x.Signup:yyyy-MM-dd}",
                        ImageUrl = x.Assets.Image.FixedUserAssetUri,
                        SearchObject = x
                    }).ToList());
                    result.Add(usersGroup);
                }
            }

            Entities = result;

            IsRunningBackgroundTask = false;
        }

        public bool CanSearch(object parameter) => !IsRunningBackgroundTask;

        private async Task NavigateTo(Entity entity)
        {
            if (entity.SearchObject is Game game)
                await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
            else if (entity.SearchObject is User user)
                await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
        }
    }
}
