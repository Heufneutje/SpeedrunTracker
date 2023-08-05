using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public class SearchEntityViewModel : BaseNetworkActionViewModel
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUserRepository _userRepository;
        private readonly SettingsViewModel _settingsViewModel;

        public ICommand SearchCommand => new AsyncRelayCommand<string>(Search, CanSearch);
        public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateTo);

        public SearchEntityViewModel(IGamesRepository gamesRepository, IUserRepository userRepository, IToastService toastService, SettingsViewModel settingsViewModel) : base(toastService)
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

            try
            {
                ObservableCollection<EntityGroup> result = new();

                if (_settingsViewModel.EnableGameSearch)
                {
                    PagedData<List<Game>> apiData = await ExecuteNetworkTask(_gamesRepository.SearchGamesAsync(query.Trim()));
                    if (apiData == null)
                        return;

                    IEnumerable<Game> games = apiData.Data.OrderBy(x => x.IsRomhack);
                    if (games.Any())
                    {
                        EntityGroup gamesGroup = new(EntityType.Games, games.Select(x => new Entity()
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
                    PagedData<List<User>> apiData = await ExecuteNetworkTask(_userRepository.SearchUsersAsync(query.Trim()));
                    if (apiData == null)
                        return;

                    if (apiData.Data.Any())
                    {
                        EntityGroup usersGroup = new(EntityType.Users, apiData.Data.Select(x => new Entity()
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
            }
            finally
            {
                IsRunningBackgroundTask = false;
            }
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
