using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class GameSeriesDetailViewModel : BaseFollowViewModel<GameSeries>
{
    private readonly IGameSeriesService _gameSeriesService;
    private int _offset;
    private bool _hasReachedEnd;

    public GameSeries? Series
    {
        get => _followEntity;
        set
        {
            if (_followEntity != value)
            {
                _followEntity = value;
                NotifyPropertyChanged();
            }
        }
    }

    private Game? _selectedGame;

    public Game? SelectedGame
    {
        get => _selectedGame;
        set
        {
            if (_selectedGame != value)
            {
                _selectedGame = value;
                NotifyPropertyChanged();
            }
        }
    }

    public RangedObservableCollection<Game> Games { get; set; }

    public ICommand LoadMoreCommand => new AsyncRelayCommand(LoadGamesAsync);

    public ICommand NavigateToGameCommand => new AsyncRelayCommand(NavigateToGameAsync);

    public override ShareDetails ShareDetails => new(Series?.Weblink, Series?.Names?.International);

    public GameSeriesDetailViewModel(IGameSeriesService gameSeriesService, ILocalFollowService followService, IShareService shareService, IToastService toastService, IPopupService popupService) : base(followService, shareService, toastService, popupService)
    {
        Games = [];
        _gameSeriesService = gameSeriesService;
    }

    protected override Task FollowAsync(GameSeries entity) => _followService.FollowSeriesAsync(entity);

    public async Task LoadGamesAsync()
    {
        if (_hasReachedEnd || Series == null)
            return;

        try
        {
            PagedData<List<Game>>? games = await ExecuteNetworkTask(_gameSeriesService.GetGameSeriesEntriesAsync(Series.Id, _offset));
            if (games == null)
                return;

            Games.AddRange(games.Data);

            if (games.Pagination.Size == 0)
                _hasReachedEnd = true;
            else
                _offset += games.Pagination.Size;
        }
        finally
        {
            CloseActivityIndicator();
        }
    }

    private async Task NavigateToGameAsync()
    {
        if (_selectedGame == null)
            return;

        ShowActivityIndicator();
        await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", _selectedGame);
        SelectedGame = null;
    }
}
