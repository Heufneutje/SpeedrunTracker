using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class UserDetailsViewModel : BaseFollowViewModel<User>
{
    private readonly IBrowserService _browserService;
    private readonly IUserRepository _userRepository;
    private bool? _isCurrentlyShowingLevels;
    private List<LeaderboardEntry> _allPersonalBests;

    public User User
    {
        get => _followEntity;
        set
        {
            if (_followEntity != value)
            {
                _followEntity = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DisplayName));
                NotifyPropertyChanged(nameof(CountryImageSource));
            }
        }
    }

    private ObservableCollection<UserPersonalBestsGroup> _personalBests;

    public ObservableCollection<UserPersonalBestsGroup> PersonalBests
    {
        get => _personalBests;
        set
        {
            if (_personalBests != value)
            {
                _personalBests = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowRuns));
            }
        }
    }

    private LeaderboardEntry _selectedEntry;

    public LeaderboardEntry SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            if (_selectedEntry != value)
            {
                _selectedEntry = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string DisplayName => User?.DisplayName;

    public string CountryImageSource => $"flags/{User?.Location?.Country?.Code?.Replace("/", "_")}_flag";

    public bool ShowRuns => PersonalBests?.Any() == true || IsRunningBackgroundTask;

    public ICommand OpenUrlCommand => new AsyncRelayCommand<string>(OpenUrl);

    public ICommand NavigateToRunCommand => new AsyncRelayCommand(NavigateToRun);

    public ICommand LoadFullGamePersonalBestsCommand => new AsyncRelayCommand(LoadFullGamePersonalBests);

    public ICommand LoadLevelPersonalBestsCommand => new AsyncRelayCommand(LoadLevelPersonalBests);

    public UserDetailsViewModel(IBrowserService browserService, IUserRepository userRepository, ILocalFollowService followService, IToastService toastService) : base(followService, toastService)
    {
        _browserService = browserService;
        _userRepository = userRepository;
        PersonalBests = new ObservableCollection<UserPersonalBestsGroup>();
    }

    private Task LoadFullGamePersonalBests() => LoadPersonalBests(false);

    private Task LoadLevelPersonalBests() => LoadPersonalBests(true);

    private async Task LoadPersonalBests(bool showLevels)
    {
        IsRunningBackgroundTask = true;
        try
        {
            if (_isCurrentlyShowingLevels == true && showLevels || _isCurrentlyShowingLevels == false && !showLevels)
                return;

            NotifyPropertyChanged(nameof(ShowRuns));
            _isCurrentlyShowingLevels = showLevels;

            _allPersonalBests ??= (await ExecuteNetworkTask(_userRepository.GetUserPersonalBestsAsync(User.Id)))?.Data;
            if (_allPersonalBests == null)
                return;

            IEnumerable<LeaderboardEntry> filteredBests = showLevels ? _allPersonalBests.Where(x => x.Run.LevelId != null) : _allPersonalBests.Where(x => x.Run.LevelId == null);

            Dictionary<string, User> players = new();
            List<UserPersonalBestsGroup> groups = new();
            foreach (IGrouping<string, LeaderboardEntry> group in filteredBests.GroupBy(x => x.Run.GameId))
            {
                List<LeaderboardEntry> entries = group.ToList();
                foreach (LeaderboardEntry entry in entries)
                {
                    foreach (KeyValuePair<string, string> valuePair in entry.Run.Values)
                    {
                        Variable variable = entry.Category.Data.Variables.Data.FirstOrDefault(x => x.Id == valuePair.Key);
                        if (variable == null || !variable.Values.Values.ContainsKey(valuePair.Value))
                            continue;

                        VariableValue value = variable.Values.Values[valuePair.Value];
                        entry.Run.Variables.Add(new(variable.Name, value.Name, variable.IsSubcategory));
                    }

                    for (int i = 0; i < entry.Run.Players.Count; i++)
                    {
                        if (entry.Run.Players[i].PlayerType != PlayerType.User)
                            continue;

                        string playerId = entry.Run.Players[i].Id;
                        if (playerId == User.Id)
                            entry.Run.Players[i] = User;
                        else
                        {
                            if (!players.ContainsKey(playerId))
                                players.Add(playerId, await GetRunUserAsync(playerId));

                            entry.Run.Players[i] = players[playerId];
                        }
                    }
                }

                groups.Add(new UserPersonalBestsGroup(entries.First().Game.Data, entries));
            }

            PersonalBests = groups.AsObservableCollection();
        }
        finally
        {
            IsRunningBackgroundTask = false;
        }
    }

    private async Task NavigateToRun()
    {
        User examiner = null;
        if (SelectedEntry.Run.Status.ExaminerId != null)
            examiner = await GetRunUserAsync(SelectedEntry.Run.Status.ExaminerId);

        RunDetails runDetails = new()
        {
            Category = SelectedEntry.Category.Data,
            GameAssets = SelectedEntry.Game.Data.Assets,
            Examiner = examiner,
            Level = SelectedEntry.Level,
            Place = SelectedEntry.Place,
            Platform = SelectedEntry.Platform,
            Run = SelectedEntry.Run,
            Variables = SelectedEntry.Run.Variables
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        SelectedEntry = null;
    }

    private async Task OpenUrl(string url)
    {
        await _browserService.OpenAsync(url);
    }

    private async Task<User> GetRunUserAsync(string userId)
    {
        return (await ExecuteNetworkTask(_userRepository.GetUserAsync(userId)))?.Data ?? User.GetUserNotFoundPlaceholder();
    }

    protected override Task FollowAsync(User entity) => _followService.FollowUserAsync(entity);
}
