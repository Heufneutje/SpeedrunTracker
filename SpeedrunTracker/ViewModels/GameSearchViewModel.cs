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
        public ICommand SearchCommand => new AsyncRelayCommand<string>(Search, CanSearch);
        public ICommand NavigateToCommand => new AsyncRelayCommand<Game>(NavigateTo);

        public GameSearchViewModel(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
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
            SearchResultGroup gamesGroup = new SearchResultGroup(Model.Enum.SearchType.Games, (await _gamesRepository.SearchGamesAsync(query.Trim())).Data);
            result.Add(gamesGroup);
            SearchResults = result;

            IsRunningBackgroundTask = false;
        }

        public bool CanSearch(object parameter) => !IsRunningBackgroundTask;

        private async Task NavigateTo(Game game)
        {
            await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", game);
        }
    }
}
