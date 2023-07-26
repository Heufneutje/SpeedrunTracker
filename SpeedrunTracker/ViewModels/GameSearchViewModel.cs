using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public class GameSearchViewModel : BaseViewModel
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUserRepository _userRepository;

        private readonly SettingsViewModel _settingsViewModel;

        public ICommand SearchCommand => new AsyncRelayCommand<string>(Search, CanSearch);
        public ICommand NavigateToCommand => new AsyncRelayCommand<SearchResult>(NavigateTo);

        public GameSearchViewModel(IGamesRepository gamesRepository, IUserRepository userRepository, SettingsViewModel settingsViewModel)
        {
            _gamesRepository = gamesRepository;
            _userRepository = userRepository;
            _settingsViewModel = settingsViewModel;
        }

        private ObservableCollection<SearchResultGroup> _searchResults;

        public ObservableCollection<SearchResultGroup> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                NotifyPropertyChanged();
            }
        }

        public async Task Search(string query)
        {
            IsRunningBackgroundTask = true;

            ObservableCollection<SearchResultGroup> result = new ObservableCollection<SearchResultGroup>();

            if (_settingsViewModel.EnableGameSearch)
            {
                List<Game> games = (await _gamesRepository.SearchGamesAsync(query.Trim())).Data;
                if (games.Any())
                {
                    SearchResultGroup gamesGroup = new SearchResultGroup(Model.Enum.SearchType.Games, games.Select(x => new SearchResult()
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
                List<User> users = (await _userRepository.SearchUsersAsync(query.Trim())).Data;
                if (users.Any())
                {
                    SearchResultGroup usersGroup = new SearchResultGroup(Model.Enum.SearchType.Users, users.Select(x => new SearchResult()
                    {
                        Title = x.Names.International,
                        Subtitle = $"Registered: {x.Signup:yyyy-MM-dd}",
                        ImageUrl = x.Assets.Image.FixedUserAssetUri,
                        SearchObject = x
                    }).ToList());
                    result.Add(usersGroup);
                }
            }

            SearchResults = result;

            IsRunningBackgroundTask = false;
        }

        public bool CanSearch(object parameter) => !IsRunningBackgroundTask;

        private async Task NavigateTo(SearchResult result)
        {
            if (result.SearchObject is Game game)
                await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
        }
    }
}
