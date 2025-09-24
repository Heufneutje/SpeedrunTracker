using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class GameDetailViewModel : BaseFollowViewModel<Game>
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
    private bool _replaceEntries;

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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Platforms))]
    [NotifyPropertyChangedFor(nameof(BackgroundUri))]
    private Game? _game;

    [ObservableProperty]
    private List<Category>? _categories;

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasIndividualLevels))]

    private List<Level>? _levels;

    [ObservableProperty]
    private Level? _selectedLevel;

    [ObservableProperty]
    private ObservableCollection<VariableViewModel>? _variables;

    private Leaderboard? _leaderboard;
    public RangedObservableCollection<LeaderboardEntry> LeaderboardEntries { get; set; }

    public bool HasIndividualLevels => Levels is not null && Levels.Count > 1;

    [ObservableProperty]
    private LeaderboardEntry? _selectedLeaderboardEntry;

    public string Platforms
    {
        get
        {
            if (Game?.Platforms is null)
                return string.Empty;

            return string.Join(", ", Game.Platforms.Data.Select(x => x.Name));
        }
    }
    public override ShareDetails ShareDetails => new(Game?.SecureWeblink, Game?.Names?.International);

    public string? BackgroundUri =>
        _settingsService.UserSettings.DisplayBackgrounds == true ? Game?.Assets?.Background?.SecureUri : null;

    partial void OnSelectedCategoryChanged(Category? value)
    {
        UpdateVariables();
    }

    partial void OnLevelsChanged(List<Level>? value)
    {
        SelectedLevel = value?.FirstOrDefault();
    }

    partial void OnSelectedLevelChanged(Level? value)
    {
        Categories = string.IsNullOrEmpty(value?.Id)
                ? (_fullGameCategories ?? []).ToList()
                : (_levelCategories ?? []).ToList();
    }

    partial void OnCategoriesChanged(List<Category>? value)
    {
        UpdateVariables();
        SelectedCategory = value?.FirstOrDefault();
    }

    public async Task<bool> LoadCategoriesAsync()
    {
        if (Game is null)
            return false;

        List<Category>? categories = await ExecuteNetworkTask(_gameService.GetGameCategoriesAsync(Game.Id));
        if (categories is null)
            return false;

        _fullGameCategories = categories.Where(x => x.Type == CategoryType.PerGame);
        _levelCategories = categories.Where(x => x.Type == CategoryType.PerLevel);
        return true;
    }

    public async Task<bool> LoadLevelsAsync()
    {
        if (Game is null)
            return false;

        List<Level> allLevels = new() { new() { Name = AppStrings.GameDetailPageFullGameButton } };
        List<Level>? gameLevels = await ExecuteNetworkTask(_gameService.GetGameLevelsAsync(Game.Id));
        if (gameLevels is null)
            return false;

        allLevels.AddRange(gameLevels);
        Levels = allLevels.ToList();
        return true;
    }

    public async Task<bool> LoadVariablesAsync()
    {
        if (Game is null)
            return false;

        _allVariables = await ExecuteNetworkTask(_gameService.GetGameVariablesAsync(Game.Id));
        return _allVariables is not null;
    }

    [RelayCommand]
    public async Task LoadLeaderboardAsync()
    {
        if (Game is null)
            return;

        ShowActivityIndicator();

        _leaderboardEntriesVisible = 0;
        _leaderboard = null;
        _replaceEntries = true;

        List<string> variableValues = new();
        string variables = string.Empty;

        if (Variables is not null)
        {
            foreach (VariableViewModel vm in Variables)
                variableValues.Add($"var-{vm.VariableId}={vm.SelectedValue?.Id}");
            variables = string.Join('&', variableValues);
        }

        if (SelectedCategory is not null)
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

            if (_leaderboard is not null)
                DisplayLeaderboardEntries();
        }

        CloseActivityIndicator();
    }

    [RelayCommand]
    private void DisplayLeaderboardEntries()
    {
        if (_leaderboard is null)
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

        if (_replaceEntries)
        {
            LeaderboardEntries.ReplaceRange(pagedEntries);
            _replaceEntries = false;
        }
        else
            LeaderboardEntries.AddRange(pagedEntries);

        _leaderboardEntriesVisible += _leaderboardEntriesStepSize;
    }

    [RelayCommand]
    private async Task NavigateToRunAsync()
    {
        if (SelectedLeaderboardEntry is null || Game is null)
            return;

        Category? category = Categories?.FirstOrDefault(x => x.Id == SelectedLeaderboardEntry.Run.CategoryId);
        if (category is null)
            return;

        ShowActivityIndicator();

        Level? level = Levels?.FirstOrDefault(x => x.Id == SelectedLeaderboardEntry.Run.LevelId);
        GamePlatform? platform = Game.Platforms.Data.Find(x =>
            x.Id == SelectedLeaderboardEntry.Run.System?.PlatformId
        );

        User? examiner = null;
        string? examinerId = SelectedLeaderboardEntry.Run.Status?.ExaminerId;
        if (examinerId is not null)
            examiner =
                Game.Moderators.Data.Find(x => x.Id == examinerId)
                ?? await ExecuteNetworkTask(_userService.GetUserAsync(examinerId))
                ?? User.GetUserNotFoundPlaceholder();

        if (!SelectedLeaderboardEntry.Run.Variables.Any())
            foreach (KeyValuePair<string, string> valuePair in SelectedLeaderboardEntry.Run.Values)
            {
                Variable? variable = _allVariables?.FirstOrDefault(x => x.Id == valuePair.Key);
                if (variable is null)
                    continue;

                VariableValue value = variable.Values.Values.FirstOrDefault(x => x.Key == valuePair.Value).Value;
                if (value is null)
                    continue;

                SelectedLeaderboardEntry.Run.Variables.Add(new(variable.Name, value.Name, variable.IsSubcategory));
            }

        RunDetails runDetails = new()
        {
            Category = category,
            GameAssets = Game.Assets,
            Examiner = examiner,
            Level = level,
            Place = SelectedLeaderboardEntry.Place,
            Platform = platform,
            Ruleset = Game.Ruleset,
            Run = SelectedLeaderboardEntry.Run,
            Variables = SelectedLeaderboardEntry.Run.Variables,
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        SelectedLeaderboardEntry = null;
    }

    [RelayCommand]
    private void ShowImagePopup()
    {
        if (Game?.Assets?.CoverSmall?.SecureUri is not null)
            ShowPopup<ImagePopupViewModel>(vm => vm.ImageSource = Game.Assets.CoverSmall.SecureUri);
    }

    private void UpdateVariables()
    {
        List<VariableViewModel> variablesVMs = new();
        IEnumerable<Variable> variables = (_allVariables ?? []).Where(x => x.IsSubcategory);
        if (string.IsNullOrEmpty(SelectedLevel?.Id))
            variables = variables.Where(x =>
                x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.FullGame
            );
        else
            variables = variables.Where(x =>
                x.Scope.Type == VariableScopeType.Global
                || x.Scope.Type == VariableScopeType.AllLevels
                || x.Scope.Type == VariableScopeType.SingleLevel && x.Scope.Level == SelectedLevel?.Id
            );

        foreach (Variable variable in variables.Where(x => x.Category is null || x.Category == SelectedCategory?.Id))
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
                    .ToList(),
            };
            variablesVMs.Add(vm);
        }

        Variables = variablesVMs.AsObservableCollection();
    }

    partial void OnGameChanged(Game? value)
    {
        _followEntity = value;
    }

    protected override Task FollowAsync(Game entity) => _followService.FollowGameAsync(entity);
}
