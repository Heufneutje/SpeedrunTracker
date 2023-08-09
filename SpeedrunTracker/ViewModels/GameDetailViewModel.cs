using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class GameDetailViewModel : BaseFollowViewModel<Game>
{
    private readonly IGamesService _gamesService;
    private readonly ILeaderboardService _leaderboardService;
    private readonly IUserService _userService;
    private readonly SettingsViewModel _settingsViewModel;
    private IEnumerable<Category> _fullGameCategories;
    private IEnumerable<Category> _levelCategories;
    private IEnumerable<Variable> _allVariables;
    private int _leaderboardEntriesVisible;
    private const int _leaderboardEntriesStepSize = 10;

    public GameDetailViewModel(IGamesService gamesService, ILeaderboardService leaderboardService, IUserService userService, ILocalFollowService followService, IToastService toastService, SettingsViewModel settingsViewModel) : base(followService, toastService)
    {
        _gamesService = gamesService;
        _leaderboardService = leaderboardService;
        _userService = userService;
        _settingsViewModel = settingsViewModel;
        LeaderboardEntries = new RangeObservableCollection<LeaderboardEntry>();
    }

    public Game Game
    {
        get => _followEntity;
        set
        {
            _followEntity = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Platforms));
        }
    }

    private ObservableCollection<Category> _categories;

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set
        {
            if (_categories.SequenceEqualOrNull(value))
            {
                if (_allVariables.Any(x => x.Scope.Type == VariableScopeType.SingleLevel))
                    UpdateVariables();
                return;
            }

            _categories = value;
            NotifyPropertyChanged();
            SelectedCategory = _categories.FirstOrDefault();
        }
    }

    private Category _selectedCategory;

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory == value)
                return;

            _selectedCategory = value;

            UpdateVariables();
            NotifyPropertyChanged();
        }
    }

    private ObservableCollection<Level> _levels;

    public ObservableCollection<Level> Levels
    {
        get => _levels;
        set
        {
            if (_levels.SequenceEqualOrNull(value))
                return;

            _levels = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(HasIndividualLevels));
            SelectedLevel = Levels.First();
        }
    }

    private Level _selectedLevel;

    public Level SelectedLevel
    {
        get => _selectedLevel;
        set
        {
            if (_selectedLevel == value)
                return;

            _selectedLevel = value;
            NotifyPropertyChanged();
            Categories = string.IsNullOrEmpty(value.Id) ? _fullGameCategories.AsObservableCollection() : _levelCategories.AsObservableCollection();
        }
    }

    private ObservableCollection<VariableViewModel> _variables;

    public ObservableCollection<VariableViewModel> Variables
    {
        get => _variables;
        set
        {
            if (_variables.SequenceEqualOrNull(value))
                return;

            _variables = value;
            NotifyPropertyChanged();
        }
    }

    private Leaderboard _leaderboard;
    public RangeObservableCollection<LeaderboardEntry> LeaderboardEntries { get; set; }

    private bool _isLoadingLeaderboard;

    public bool IsLoadingLeaderboard
    {
        get => _isLoadingLeaderboard;
        set
        {
            _isLoadingLeaderboard = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(IsLeaderboardVisible));
        }
    }

    public bool IsLeaderboardVisible => IsLoadingLeaderboard || _leaderboard != null;

    public bool HasIndividualLevels => _levels != null && _levels.Count > 1;

    private LeaderboardEntry _selectedLeaderboardEntry;

    public LeaderboardEntry SelectedLeaderboardEntry
    {
        get => _selectedLeaderboardEntry;
        set
        {
            if (_selectedLeaderboardEntry != value)
            {
                _selectedLeaderboardEntry = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Platforms
    {
        get
        {
            if (Game?.Platforms == null)
                return string.Empty;

            return string.Join(", ", Game.Platforms.Data.Select(x => x.Name));
        }
    }

    public ICommand ShowLeaderboardCommand => new AsyncRelayCommand(LoadLeaderboardAsync);

    public ICommand NavigateToRunCommand => new AsyncRelayCommand(NavigateToRunAsync);

    public ICommand DisplayLeaderboardEntriesCommand => new Command(DisplayLeaderboardEntries);

    public async Task<bool> LoadCategoriesAsync()
    {
        List<Category> categories = (await ExecuteNetworkTask(_gamesService.GetGameCategoriesAsync(Game.Id)))?.Data;
        if (categories == null)
            return false;

        _fullGameCategories = categories.Where(x => x.Type == CategoryType.PerGame);
        _levelCategories = categories.Where(x => x.Type == CategoryType.PerLevel);
        return true;
    }

    public async Task<bool> LoadLevelsAsync()
    {
        List<Level> allLevels = new() { new() { Name = "Full Game" } };
        List<Level> gameLevels = (await ExecuteNetworkTask(_gamesService.GetGameLevelsAsync(Game.Id)))?.Data;
        if (gameLevels == null)
            return false;

        allLevels.AddRange(gameLevels);
        Levels = allLevels.AsObservableCollection();
        return true;
    }

    public async Task<bool> LoadVariablesAsync()
    {
        _allVariables = (await ExecuteNetworkTask(_gamesService.GetGameVariablesAsync(Game.Id)))?.Data;
        return _allVariables != null;
    }

    public async Task LoadLeaderboardAsync()
    {
        IsLoadingLeaderboard = true;
        _leaderboardEntriesVisible = 0;
        _leaderboard = null;
        LeaderboardEntries.Clear();

        List<string> variableValues = new();
        foreach (VariableViewModel vm in Variables)
            variableValues.Add($"var-{vm.VariableId}={vm.SelectedValue.Id}");
        string variables = string.Join('&', variableValues);

        if (!string.IsNullOrEmpty(variables))
            variables = $"&{variables}";

        _leaderboard = (string.IsNullOrEmpty(SelectedLevel.Id) ?
            await ExecuteNetworkTask(_leaderboardService.GetFullGameLeaderboardAsync(Game.Id, SelectedCategory.Id, variables, _settingsViewModel.MaxLeaderboardResults)) :
            await ExecuteNetworkTask(_leaderboardService.GetLevelLeaderboardAsync(Game.Id, SelectedLevel.Id, SelectedCategory.Id, variables, _settingsViewModel.MaxLeaderboardResults)))?.Data;

        if (_leaderboard != null)
            DisplayLeaderboardEntries();

        IsLoadingLeaderboard = false;
    }

    private void DisplayLeaderboardEntries()
    {
        if (_leaderboard == null)
            return;

        IEnumerable<LeaderboardEntry> pagedEntries = _leaderboard.Runs.Skip(_leaderboardEntriesVisible).Take(_leaderboardEntriesStepSize);
        foreach (LeaderboardEntry entry in pagedEntries)
        {
            for (int i = 0; i < entry.Run.Players.Count; i++)
            {
                if (entry.Run.Players[i].PlayerType == PlayerType.User)
                    entry.Run.Players[i] = _leaderboard.Players.Data.FirstOrDefault(x => x.Id == entry.Run.Players[i].Id);
                entry.TrophyAsset = Game.Assets.GetTrophyAsset(entry.Place);
            }
        }

        LeaderboardEntries.AddRange(pagedEntries);
        _leaderboardEntriesVisible += _leaderboardEntriesStepSize;
    }

    private async Task NavigateToRunAsync()
    {
        if (_selectedLeaderboardEntry == null)
            return;

        Category category = _categories.FirstOrDefault(x => x.Id == _selectedLeaderboardEntry.Run.CategoryId);
        Level level = _levels.FirstOrDefault(x => x.Id == _selectedLeaderboardEntry.Run.LevelId);
        GamePlatform platform = Game.Platforms.Data.FirstOrDefault(x => x.Id == _selectedLeaderboardEntry.Run.System.PlatformId);

        User examiner = null;
        string examinerId = _selectedLeaderboardEntry.Run.Status.ExaminerId;
        if (examinerId != null)
            examiner = Game.Moderators.Data.FirstOrDefault(x => x.Id == examinerId) ?? (await ExecuteNetworkTask(_userService.GetUserAsync(examinerId)))?.Data ?? User.GetUserNotFoundPlaceholder();

        foreach (KeyValuePair<string, string> valuePair in _selectedLeaderboardEntry.Run.Values)
        {
            Variable variable = _allVariables.FirstOrDefault(x => x.Id == valuePair.Key);
            if (variable == null)
                continue;

            VariableValue value = variable.Values.Values.FirstOrDefault(x => x.Key == valuePair.Value).Value;
            if (value == null)
                continue;

            _selectedLeaderboardEntry.Run.Variables.Add(new(variable.Name, value.Name, variable.IsSubcategory));
        }

        RunDetails runDetails = new()
        {
            Category = category,
            GameAssets = Game.Assets,
            Examiner = examiner,
            Level = level,
            Place = _selectedLeaderboardEntry.Place,
            Platform = platform,
            Ruleset = Game.Ruleset,
            Run = _selectedLeaderboardEntry.Run,
            Variables = _selectedLeaderboardEntry.Run.Variables,
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        SelectedLeaderboardEntry = null;
    }

    private void UpdateVariables()
    {
        List<VariableViewModel> variablesVMs = new();
        IEnumerable<Variable> variables = _allVariables.Where(x => x.IsSubcategory);
        if (string.IsNullOrEmpty(SelectedLevel.Id))
            variables = variables.Where(x => x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.FullGame);
        else
            variables = variables.Where(x => x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.AllLevels || x.Scope.Type == VariableScopeType.SingleLevel && x.Scope.Level == SelectedLevel.Id);

        foreach (Variable variable in variables.Where(x => x.Category == null || x.Category == SelectedCategory.Id))
        {
            VariableViewModel vm = new()
            {
                VariableId = variable.Id,
                Name = variable.Name,
                Values = variable.Values.Values.Select(x => new ViewVariableValue()
                {
                    Id = x.Key,
                    Name = x.Value.Name,
                    Rules = x.Value.Rules
                }).AsObservableCollection()
            };
            variablesVMs.Add(vm);
        }

        Variables = variablesVMs.AsObservableCollection();
    }

    protected override Task FollowAsync(Game entity) => _followService.FollowGameAsync(entity);
}
