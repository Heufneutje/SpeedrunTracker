using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public partial class UserDetailsViewModel : BaseFollowViewModel<User>
{
    private readonly IBrowserService _browserService;
    private readonly IUserService _userService;
    private readonly ILocalSettingsService _settingsService;
    private bool? _isCurrentlyShowingLevels;
    private List<LeaderboardEntry>? _allPersonalBests;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayName))]
    [NotifyPropertyChangedFor(nameof(CountryImageSource))]
    private User? _user;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowRuns))]
    private List<UserPersonalBestsGroup>? _personalBests;

    [ObservableProperty]
    private UserRunViewModel? _selectedEntry;

    public string? DisplayName => User?.DisplayName;

    public string CountryImageSource => $"flags/{User?.Location?.Country?.Code?.Replace("/", "_")}_flag";

    public bool ShowRuns => PersonalBests?.Any() == true || IsRunningBackgroundTask;

    public override ShareDetails ShareDetails => new(User?.Weblink, User?.Names?.International);

    public UserDetailsViewModel(
        IBrowserService browserService,
        IUserService userService,
        ILocalFollowService followService,
        IShareService shareService,
        IToastService toastService,
        ILocalSettingsService settingsService,
        IPopupService popupService
    )
        : base(followService, shareService, toastService, popupService)
    {
        _browserService = browserService;
        _userService = userService;
        _settingsService = settingsService;
        PersonalBests = [];
    }

    partial void OnUserChanged(User? value)
    {
        _followEntity = value;
    }

    [RelayCommand]
    private Task LoadFullGamePersonalBests() => LoadPersonalBestsAsync(false);

    [RelayCommand]
    private Task LoadLevelPersonalBests() => LoadPersonalBestsAsync(true);

    private async Task LoadPersonalBestsAsync(bool showLevels)
    {
        await ShowActivityIndicatorAsync();
        try
        {
            if (
                _isCurrentlyShowingLevels == true && showLevels
                || _isCurrentlyShowingLevels == false && !showLevels
                || User is null
            )
                return;

            OnPropertyChanged(nameof(ShowRuns));
            _isCurrentlyShowingLevels = showLevels;

            _allPersonalBests ??= await ExecuteNetworkTask(_userService.GetUserPersonalBestsAsync(User.Id));
            if (_allPersonalBests is null)
                return;

            IEnumerable<LeaderboardEntry> filteredBests = showLevels
                ? _allPersonalBests.Where(x => x.Run.LevelId is not null)
                : _allPersonalBests.Where(x => x.Run.LevelId is null);

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

                BaseGame? game = entries[0].Game?.Data;
                if (game is not null)
                    groups.Add(
                        new UserPersonalBestsGroup(
                            game,
                            entries.Select(x => new UserRunViewModel(x, _settingsService.UserSettings.DateFormat))
                        )
                    );
            }

            PersonalBests = groups.ToList();
        }
        finally
        {
            await CloseActivityIndicatorAsync();
        }
    }

    private static void ParseRunVariables(LeaderboardEntry entry)
    {
        foreach (KeyValuePair<string, string> valuePair in entry.Run.Values)
        {
            Variable? variable = entry.Category?.Data.Variables.Data.Find(x => x.Id == valuePair.Key);
            if (variable is null || !variable.Values.Values.ContainsKey(valuePair.Value))
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

    [RelayCommand]
    private async Task NavigateToRunAsync()
    {
        User? examiner = null;
        LeaderboardEntry? leaderboardEntry = SelectedEntry?.Entry;
        if (leaderboardEntry is null)
            return;

        await ShowActivityIndicatorAsync();

        if (leaderboardEntry.Run.Status?.ExaminerId is not null)
            examiner = await GetRunUserAsync(leaderboardEntry.Run.Status.ExaminerId);

        RunDetails runDetails = new()
        {
            Category = leaderboardEntry.Category?.Data,
            GameAssets = leaderboardEntry.Game?.Data.Assets,
            Examiner = examiner,
            Level = leaderboardEntry.Level,
            Place = leaderboardEntry.Place,
            Platform = leaderboardEntry.Platform,
            Run = leaderboardEntry.Run,
            Variables = leaderboardEntry.Run.Variables,
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        SelectedEntry = null;
    }

    [RelayCommand]
    private async Task OpenUrlAsync(string? url)
    {
        if (!string.IsNullOrEmpty(url))
            await _browserService.OpenAsync(url);
    }

    [RelayCommand]
    private async Task ShowAvatarPopupAsync()
    {
        if (User?.Assets?.Image is not null)
            await ShowPopupAsync<ImagePopupViewModel>(new()
            {
                [nameof(ImagePopupViewModel.ImageSource)] = User.Assets.Image.Uri
            });
    }

    private async Task<User> GetRunUserAsync(string userId)
    {
        return await ExecuteNetworkTask(_userService.GetUserAsync(userId)) ?? User.GetUserNotFoundPlaceholder();
    }

    protected override Task FollowAsync(User entity) => _followService.FollowUserAsync(entity);
}
