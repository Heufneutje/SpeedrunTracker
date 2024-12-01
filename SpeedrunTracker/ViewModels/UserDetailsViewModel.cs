using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class UserDetailsViewModel : BaseFollowViewModel<User>
{
    private readonly IBrowserService _browserService;
    private readonly IUserService _userService;
    private readonly ILocalSettingsService _settingsService;
    private bool? _isCurrentlyShowingLevels;
    private List<LeaderboardEntry>? _allPersonalBests;

    public User? User
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

    private ObservableCollection<UserPersonalBestsGroup>? _personalBests;

    public ObservableCollection<UserPersonalBestsGroup> PersonalBests
    {
        get => _personalBests ?? [];
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

    private UserRunViewModel? _selectedEntry;

    public UserRunViewModel? SelectedEntry
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

    public string? DisplayName => User?.DisplayName;

    public string CountryImageSource => $"flags/{User?.Location?.Country?.Code?.Replace("/", "_")}_flag";

    public bool ShowRuns => PersonalBests?.Any() == true || IsRunningBackgroundTask;

    public ICommand OpenUrlCommand => new AsyncRelayCommand<string>(OpenUrl);

    public ICommand NavigateToRunCommand => new AsyncRelayCommand(NavigateToRun);

    public ICommand LoadFullGamePersonalBestsCommand => new AsyncRelayCommand(LoadFullGamePersonalBests);

    public ICommand LoadLevelPersonalBestsCommand => new AsyncRelayCommand(LoadLevelPersonalBests);

    public override ShareDetails ShareDetails => new(User?.Weblink, User?.Names?.International);

    public UserDetailsViewModel(IBrowserService browserService, IUserService userService, ILocalFollowService followService, IShareService shareService, IToastService toastService, ILocalSettingsService settingsService) : base(followService, shareService, toastService)
    {
        _browserService = browserService;
        _userService = userService;
        _settingsService = settingsService;
        PersonalBests = [];
    }

    private Task LoadFullGamePersonalBests() => LoadPersonalBests(false);

    private Task LoadLevelPersonalBests() => LoadPersonalBests(true);

    private async Task LoadPersonalBests(bool showLevels)
    {
        IsRunningBackgroundTask = true;
        try
        {
            if (_isCurrentlyShowingLevels == true && showLevels || _isCurrentlyShowingLevels == false && !showLevels || User == null)
                return;

            NotifyPropertyChanged(nameof(ShowRuns));
            _isCurrentlyShowingLevels = showLevels;

            _allPersonalBests ??= await ExecuteNetworkTask(_userService.GetUserPersonalBestsAsync(User.Id));
            if (_allPersonalBests == null)
                return;

            IEnumerable<LeaderboardEntry> filteredBests = showLevels ? _allPersonalBests.Where(x => x.Run.LevelId != null) : _allPersonalBests.Where(x => x.Run.LevelId == null);

            Dictionary<string, User> players = [];
            List<UserPersonalBestsGroup> groups = [];
            foreach (IGrouping<string?, LeaderboardEntry> group in filteredBests.GroupBy(x => x.Run.GameId))
            {
                List<LeaderboardEntry> entries = group.ToList();
                foreach (LeaderboardEntry entry in group)
                {
                    ParseRunVariables(entry);
                    await ParseRunPlayersAsync(entry, players);
                }

                groups.Add(new UserPersonalBestsGroup(entries[0].Game.Data, entries.Select(x => new UserRunViewModel(x, _settingsService.UserSettings.DateFormat))));
            }

            PersonalBests = groups.AsObservableCollection();
        }
        finally
        {
            IsRunningBackgroundTask = false;
        }
    }

    private void ParseRunVariables(LeaderboardEntry entry)
    {
        foreach (KeyValuePair<string, string> valuePair in entry.Run.Values)
        {
            Variable? variable = entry.Category.Data.Variables.Data.Find(x => x.Id == valuePair.Key);
            if (variable == null || !variable.Values.Values.ContainsKey(valuePair.Value))
                continue;

            VariableValue value = variable.Values.Values[valuePair.Value];
            entry.Run.Variables.Add(new(variable.Name, value.Name, variable.IsSubcategory));
        }
    }

    private async Task ParseRunPlayersAsync(LeaderboardEntry entry, Dictionary<string, User> players)
    {
        for (int i = 0; i < entry.Run.Players.Count; i++)
        {
            if (entry.Run.Players[i].PlayerType != PlayerType.User)
                continue;

            string playerId = entry.Run.Players[i].Id;
            if (playerId == User?.Id)
                entry.Run.Players[i] = User;
            else
            {
                if (!players.ContainsKey(playerId))
                    players.Add(playerId, await GetRunUserAsync(playerId));

                entry.Run.Players[i] = players[playerId];
            }
        }
    }

    private async Task NavigateToRun()
    {
        User? examiner = null;
        LeaderboardEntry? leaderboardEntry = SelectedEntry?.Entry;
        if (leaderboardEntry == null)
            return;

        if (leaderboardEntry.Run.Status?.ExaminerId != null)
            examiner = await GetRunUserAsync(leaderboardEntry.Run.Status.ExaminerId);

        RunDetails runDetails = new()
        {
            Category = leaderboardEntry.Category.Data,
            GameAssets = leaderboardEntry.Game.Data.Assets,
            Examiner = examiner,
            Level = leaderboardEntry.Level,
            Place = leaderboardEntry.Place,
            Platform = leaderboardEntry.Platform,
            Run = leaderboardEntry.Run,
            Variables = leaderboardEntry.Run.Variables
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        SelectedEntry = null;
    }

    private async Task OpenUrl(string? url)
    {
        if (!string.IsNullOrEmpty(url))
            await _browserService.OpenAsync(url);
    }

    private async Task<User> GetRunUserAsync(string userId)
    {
        return await ExecuteNetworkTask(_userService.GetUserAsync(userId)) ?? User.GetUserNotFoundPlaceholder();
    }

    protected override Task FollowAsync(User entity) => _followService.FollowUserAsync(entity);
}
