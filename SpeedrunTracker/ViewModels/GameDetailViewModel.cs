using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SpeedrunTracker.Model.Enum;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly Dictionary<string, List<VariableViewModel>> _variableViewModels;

        // TODO: Replace with config value
        private const int _maxResults = 50;

        public GameDetailViewModel(IGamesRepository gamesRepository, ILeaderboardRepository leaderboardRepository)
        {
            _gamesRepository = gamesRepository;
            _leaderboardRepository = leaderboardRepository;
            _variableViewModels = new Dictionary<string, List<VariableViewModel>>();
        }

        private Game _game;
        public Game Game
        {
            get => _game;
            set
            {
                _game = value;
                NotifyPropertyChanged();
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
                if (_categories == value)
                    return;

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

                if (!_variableViewModels.ContainsKey(_selectedCategory.Id))
                {
                    List<VariableViewModel> variablesVMs = new List<VariableViewModel>();
                    IEnumerable<Variable> variables = _allVariables.Where(x => x.IsSubcategory);
                    if (string.IsNullOrEmpty(SelectedLevel.Id))
                        variables = variables.Where(x => x.Scope.Type != VariableScopeType.AllLevels);
                    else
                        variables = variables.Where(x => x.Scope.Type != VariableScopeType.FullGame);

                    foreach (Variable variable in variables.Where(x => x.Category == null || x.Category == value.Id))
                    {
                        VariableViewModel vm = new VariableViewModel()
                        {
                            VariableId = variable.Id,
                            Name = variable.Name,
                            Values = variable.Values.Values.Select(x => new ViewVariableValue()
                            {
                                Id = x.Key,
                                Label = x.Value.Label,
                                Rules = x.Value.Rules
                            }).AsObservableCollection()
                        };
                        variablesVMs.Add(vm);
                    }

                    _variableViewModels.Add(_selectedCategory.Id, variablesVMs);
                }

                Variables = _variableViewModels[_selectedCategory.Id].AsObservableCollection();
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Level> _levels;
        public ObservableCollection<Level> Levels
        {
            get => _levels;
            set
            {
                if (_levels == value)
                    return;

                _levels = value;
                NotifyPropertyChanged();
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
            }
        }

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
            List<string> variableValues = new List<string>();
            foreach (VariableViewModel vm in Variables)
                variableValues.Add($"var-{vm.VariableId}={vm.SelectedValue.Id}");
            string variables = string.Join('&', variableValues);

            if (!string.IsNullOrEmpty(variables))
                variables = $"&{variables}";

            Leaderboard leaderboard = (string.IsNullOrEmpty(SelectedLevel.Id) ?
                await _leaderboardRepository.GetFullGameLeaderboardAsync(Game.Id, SelectedCategory.Id, variables, _maxResults) :
                await _leaderboardRepository.GetLevelLeaderboardAsync(Game.Id, SelectedLevel.Id, SelectedCategory.Id, variables, _maxResults)).Data;

            foreach (LeaderboardEntry entry in leaderboard.Runs)
                for (int i = 0; i < entry.Run.Players.Count; i++)
                    if (entry.Run.Players[i].PlayerType == PlayerType.User)
                        entry.Run.Players[i] = leaderboard.Players.Data.FirstOrDefault(x => x.Id == entry.Run.Players[i].Id);

            Leaderboard = leaderboard;
        }
    }
}
