using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class GameDetailViewModel : BaseFollowViewModel<Game>
{
    private readonly IGameService _gameService;
    private readonly ILeaderboardService _leaderboardService;
    private readonly IUserService _userService;
    private readonly ILocalSettingsService _settingsService;
    private IEnumerable<Category>? _fullGameCategories;
    private IEnumerable<Category>? _levelCategories;
    private IEnumerable<Variable>? _allVariables;
    private int _leaderboardEntriesVisible;
    private const int _leaderboardEntriesStepSize = 10;

    public GameDetailViewModel(
        IGameService gameService,
        ILeaderboardService leaderboardService,
        IUserService userService,
        ILocalFollowService followService,
        IShareService shareService,
        IToastService toastService,
        ILocalSettingsService settingsService,
        IPopupService popupService
    )
        : base(followService, shareService, toastService, popupService)
    {
        _gameService = gameService;
        _leaderboardService = leaderboardService;
        _userService = userService;
        _settingsService = settingsService;
        LeaderboardEntries = [];
    }

    public Game? Game
    {
        get => _followEntity;
        set
        {
            _followEntity = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Platforms));
            NotifyPropertyChanged(nameof(BackgroundUri));
        }
    }

    private ObservableCollection<Category>? _categories;

    public ObservableCollection<Category> Categories
    {
        get => _categories ?? [];
        set
        {
            if (_categories != null && _categories.SequenceEqualOrNull(value))
            {
                if (_allVariables != null && _allVariables.Any(x => x.Scope.Type == VariableScopeType.SingleLevel))
                    UpdateVariables();
                return;
            }

            _categories = value;
            NotifyPropertyChanged();
            SelectedCategory = _categories.FirstOrDefault();
        }
    }

    private Category? _selectedCategory;

    public Category? SelectedCategory
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

    private ObservableCollection<Level>? _levels;

    public ObservableCollection<Level> Levels
    {
        get => _levels ?? [];
        set
        {
            if (_levels != null && _levels.SequenceEqualOrNull(value))
                return;

            _levels = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(HasIndividualLevels));
            SelectedLevel = Levels[0];
        }
    }

    private Level? _selectedLevel;

    public Level? SelectedLevel
    {
        get => _selectedLevel;
        set
        {
            if (_selectedLevel == value)
                return;

            _selectedLevel = value;
            NotifyPropertyChanged();
            Categories = string.IsNullOrEmpty(value?.Id)
                ? (_fullGameCategories ?? []).AsObservableCollection()
                : (_levelCategories ?? []).AsObservableCollection();
        }
    }

    private ObservableCollection<VariableViewModel>? _variables;

    public ObservableCollection<VariableViewModel> Variables
    {
        get => _variables ?? [];
        set
        {
            if (_variables != null && _variables.SequenceEqualOrNull(value))
                return;

            _variables = value;
            NotifyPropertyChanged();
        }
    }

    private Leaderboard? _leaderboard;
    public RangedObservableCollection<LeaderboardEntry> LeaderboardEntries { get; set; }

    public bool HasIndividualLevels => _levels != null && _levels.Count > 1;

    private LeaderboardEntry? _selectedLeaderboardEntry;

    public LeaderboardEntry? SelectedLeaderboardEntry
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

    public override ShareDetails ShareDetails => new(Game?.Weblink, Game?.Names?.International);

    public string? BackgroundUri =>
        _settingsService.UserSettings.DisplayBackgrounds == true ? Game?.Assets?.Background?.Uri : null;

    public async Task<bool> LoadCategoriesAsync()
    {
        if (Game == null)
            return false;

        List<Category>? categories = await ExecuteNetworkTask(_gameService.GetGameCategoriesAsync(Game.Id));
        if (categories == null)
            return false;

        _fullGameCategories = categories.Where(x => x.Type == CategoryType.PerGame);
        _levelCategories = categories.Where(x => x.Type == CategoryType.PerLevel);
        return true;
    }

    public async Task<bool> LoadLevelsAsync()
    {
        if (Game == null)
            return false;

        List<Level> allLevels = new() { new() { Name = "Full Game" } };
        List<Level>? gameLevels = await ExecuteNetworkTask(_gameService.GetGameLevelsAsync(Game.Id));
        if (gameLevels == null)
            return false;

        allLevels.AddRange(gameLevels);
        Levels = allLevels.AsObservableCollection();
        return true;
    }

    public async Task<bool> LoadVariablesAsync()
    {
        if (Game == null)
            return false;

        _allVariables = await ExecuteNetworkTask(_gameService.GetGameVariablesAsync(Game.Id));
        return _allVariables != null;
    }

    public async Task LoadLeaderboardAsync()
    {
        if (Game == null)
            return;

        ShowActivityIndicator();

        _leaderboardEntriesVisible = 0;
        _leaderboard = null;
        LeaderboardEntries.Clear();

        List<string> variableValues = new();
        string variables = string.Empty;

        if (Variables != null)
        {
            foreach (VariableViewModel vm in Variables)
                variableValues.Add($"var-{vm.VariableId}={vm.SelectedValue?.Id}");
            variables = string.Join('&', variableValues);
        }

        if (SelectedCategory != null)
        {
            if (!string.IsNullOrEmpty(variables))
                variables = $"&{variables}";

            _leaderboard = string.IsNullOrEmpty(SelectedLevel?.Id)
                ? await ExecuteNetworkTask(
                    _leaderboardService.GetFullGameLeaderboardAsync(
                        Game.Id,
                        SelectedCategory.Id,
                        variables,
                        _settingsService.UserSettings.MaxLeaderboardResults
                    )
                )
                : await ExecuteNetworkTask(
                    _leaderboardService.GetLevelLeaderboardAsync(
                        Game.Id,
                        SelectedLevel.Id,
                        SelectedCategory.Id,
                        variables,
                        _settingsService.UserSettings.MaxLeaderboardResults
                    )
                );

            if (_leaderboard != null)
                DisplayLeaderboardEntries();
        }

        CloseActivityIndicator();
    }

    private void DisplayLeaderboardEntries()
    {
        if (_leaderboard == null)
            return;

        IEnumerable<LeaderboardEntry> pagedEntries = _leaderboard
            .Runs.Skip(_leaderboardEntriesVisible)
            .Take(_leaderboardEntriesStepSize);
        foreach (LeaderboardEntry entry in pagedEntries)
        {
            for (int i = 0; i < entry.Run.Players.Count; i++)
            {
                if (entry.Run.Players[i].PlayerType == PlayerType.User)
                    entry.Run.Players[i] = _leaderboard.Players.Data.First(x => x.Id == entry.Run.Players[i].Id);

                entry.TrophyAsset = Game?.Assets.GetTrophyAsset(entry.Place);
            }
        }

        LeaderboardEntries.AddRange(pagedEntries);
        _leaderboardEntriesVisible += _leaderboardEntriesStepSize;
    }

    private async Task NavigateToRunAsync()
    {
        if (_selectedLeaderboardEntry == null || Game == null)
            return;

        Category? category = _categories?.FirstOrDefault(x => x.Id == _selectedLeaderboardEntry.Run.CategoryId);
        if (category == null)
            return;

        ShowActivityIndicator();

        Level? level = _levels?.FirstOrDefault(x => x.Id == _selectedLeaderboardEntry.Run.LevelId);
        GamePlatform? platform = Game.Platforms.Data.Find(x =>
            x.Id == _selectedLeaderboardEntry.Run.System?.PlatformId
        );

        User? examiner = null;
        string? examinerId = _selectedLeaderboardEntry.Run.Status?.ExaminerId;
        if (examinerId != null)
            examiner =
                Game.Moderators.Data.Find(x => x.Id == examinerId)
                ?? await ExecuteNetworkTask(_userService.GetUserAsync(examinerId))
                ?? User.GetUserNotFoundPlaceholder();

        if (!_selectedLeaderboardEntry.Run.Variables.Any())
            foreach (KeyValuePair<string, string> valuePair in _selectedLeaderboardEntry.Run.Values)
            {
                Variable? variable = _allVariables?.FirstOrDefault(x => x.Id == valuePair.Key);
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
        IEnumerable<Variable> variables = (_allVariables ?? []).Where(x => x.IsSubcategory);
        if (string.IsNullOrEmpty(_selectedLevel?.Id))
            variables = variables.Where(x =>
                x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.FullGame
            );
        else
            variables = variables.Where(x =>
                x.Scope.Type == VariableScopeType.Global
                || x.Scope.Type == VariableScopeType.AllLevels
                || x.Scope.Type == VariableScopeType.SingleLevel && x.Scope.Level == _selectedLevel?.Id
            );

        foreach (Variable variable in variables.Where(x => x.Category == null || x.Category == _selectedCategory?.Id))
        {
            VariableViewModel vm = new()
            {
                VariableId = variable.Id,
                Name = variable.Name,
                Values = variable
                    .Values.Values.Select(x => new ViewVariableValue()
                    {
                        Id = x.Key,
                        Name = x.Value.Name,
                        Rules = x.Value.Rules,
                    })
                    .AsObservableCollection(),
            };
            variablesVMs.Add(vm);
        }

        Variables = variablesVMs.AsObservableCollection();
    }

    protected override Task FollowAsync(Game entity) => _followService.FollowGameAsync(entity);
}
