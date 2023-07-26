using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SpeedrunTracker.Model.Enum;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly SettingsViewModel _settingsViewModel;

        public GameDetailViewModel(IGamesRepository gamesRepository, ILeaderboardRepository leaderboardRepository, SettingsViewModel settingsViewModel)
        {
            _gamesRepository = gamesRepository;
            _leaderboardRepository = leaderboardRepository;
            _settingsViewModel = settingsViewModel;
        }

        private Game _game;

        public Game Game
        {
            get => _game;
            set
            {
                _game = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Platforms));
            }
        }

        private IEnumerable<Category> _fullGameCategories;
        private IEnumerable<Category> _levelCategories;
        private IEnumerable<Variable> _allVariables;

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

        public Leaderboard Leaderboard
        {
            get => _leaderboard;
            set
            {
                _leaderboard = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsLeaderboardVisible));
            }
        }

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

        public ICommand NavigateToRunCommand => new AsyncRelayCommand<LeaderboardEntry>(NavigateToRun);

        public async Task LoadCategoriesAsync()
        {
            List<Category> categories = (await _gamesRepository.GetGameCategoriesAsync(Game.Id)).Data;
            _fullGameCategories = categories.Where(x => x.Type == CategoryType.PerGame);
            _levelCategories = categories.Where(x => x.Type == CategoryType.PerLevel);
        }

        public async Task LoadLevelsAsync()
        {
            List<Level> levels = new List<Level>() { new() { Name = "Full Game" } };
            levels.AddRange((await _gamesRepository.GetGameLevelsAsync(Game.Id)).Data);
            Levels = levels.AsObservableCollection();
        }

        public async Task LoadVariablesAsync()
        {
            _allVariables = (await _gamesRepository.GetGameVariablesAsync(Game.Id)).Data;
        }

        public async Task LoadLeaderboardAsync()
        {
            IsLoadingLeaderboard = true;

            List<string> variableValues = new List<string>();
            foreach (VariableViewModel vm in Variables)
                variableValues.Add($"var-{vm.VariableId}={vm.SelectedValue.Id}");
            string variables = string.Join('&', variableValues);

            if (!string.IsNullOrEmpty(variables))
                variables = $"&{variables}";

            Leaderboard leaderboard = (string.IsNullOrEmpty(SelectedLevel.Id) ?
                await _leaderboardRepository.GetFullGameLeaderboardAsync(Game.Id, SelectedCategory.Id, variables, _settingsViewModel.MaxLeaderboardResults) :
                await _leaderboardRepository.GetLevelLeaderboardAsync(Game.Id, SelectedLevel.Id, SelectedCategory.Id, variables, _settingsViewModel.MaxLeaderboardResults)).Data;

            foreach (LeaderboardEntry entry in leaderboard.Runs)
                for (int i = 0; i < entry.Run.Players.Count; i++)
                    if (entry.Run.Players[i].PlayerType == PlayerType.User)
                        entry.Run.Players[i] = leaderboard.Players.Data.FirstOrDefault(x => x.Id == entry.Run.Players[i].Id);

            Leaderboard = leaderboard;
            IsLoadingLeaderboard = false;
        }

        private async Task NavigateToRun(LeaderboardEntry entry)
        {
            Category category = _categories.FirstOrDefault(x => x.Id == entry.Run.CategoryId);
            Level level = _levels.FirstOrDefault(x => x.Id == entry.Run.LevelId);
            GamePlatform platform = Game.Platforms.Data.FirstOrDefault(x => x.Id == entry.Run.System.PlatformId);
            User examiner = Game.Moderators.Data.FirstOrDefault(x => x.Id == entry.Run.Status.ExaminerId);

            List<RunVariable> runVariables = new();
            foreach (KeyValuePair<string, string> valuePair in entry.Run.Values)
            {
                Variable variable = _allVariables.FirstOrDefault(x => x.Id == valuePair.Key);
                if (variable == null)
                    continue;

                VariableValue value = variable.Values.Values.FirstOrDefault(x => x.Key == valuePair.Value).Value;
                if (value == null)
                    continue;

                runVariables.Add(new()
                {
                    Name = variable.Name,
                    Value = value.Name,
                    IsSubcategory = variable.IsSubcategory
                });
            }

            RunDetails runDetails = new RunDetails()
            {
                Category = category,
                GameAssets = _game.Assets,
                Examiner = examiner,
                Level = level,
                Place = entry.Place,
                Platform = platform,
                Ruleset = _game.Ruleset,
                Run = entry.Run,
                Variables = runVariables,
            };

            await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
        }

        private void UpdateVariables()
        {
            List<VariableViewModel> variablesVMs = new List<VariableViewModel>();
            IEnumerable<Variable> variables = _allVariables.Where(x => x.IsSubcategory);
            if (string.IsNullOrEmpty(SelectedLevel.Id))
                variables = variables.Where(x => x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.FullGame);
            else
                variables = variables.Where(x => x.Scope.Type == VariableScopeType.Global || x.Scope.Type == VariableScopeType.AllLevels || x.Scope.Type == VariableScopeType.SingleLevel && x.Scope.Level == SelectedLevel.Id);

            foreach (Variable variable in variables.Where(x => x.Category == null || x.Category == SelectedCategory.Id))
            {
                VariableViewModel vm = new VariableViewModel()
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

        private Asset GetLeaderboardEntryAsset(int place)
        {
            return place switch
            {
                1 => Game?.Assets?.TrophyFirstPlace,
                2 => Game?.Assets?.TrophySecondPlace,
                3 => Game?.Assets?.TrophyThirdPlace,
                4 => Game?.Assets?.TrophyFouthPlace,
                _ => null,
            };
        }
    }
}
