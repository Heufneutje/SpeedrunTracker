using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public partial class GameSeriesDetailViewModel : BaseFollowViewModel<GameSeries>
{
    private readonly IGameSeriesService _gameSeriesService;
    private readonly ILocalSettingsService _settingsService;
    private int _offset;
    private bool _hasReachedEnd;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundUri))]
    private GameSeries? _series;

    [ObservableProperty]
    private Game? _selectedGame;

    public RangedObservableCollection<Game> Games { get; set; }

    public override ShareDetails ShareDetails => new(Series?.Weblink, Series?.Names?.International);

    public string? BackgroundUri =>
        _settingsService.UserSettings.DisplayBackgrounds == true ? Series?.Assets?.Background?.Uri : null;

    partial void OnSeriesChanged(GameSeries? value)
    {
        _followEntity = value;
    }

    public GameSeriesDetailViewModel(
        IGameSeriesService gameSeriesService,
        ILocalFollowService followService,
        IShareService shareService,
        IToastService toastService,
        IPopupService popupService,
        ILocalSettingsService settingsService
    )
        : base(followService, shareService, toastService, popupService)
    {
        Games = [];
        _gameSeriesService = gameSeriesService;
        _settingsService = settingsService;
    }

    protected override Task FollowAsync(GameSeries entity) => _followService.FollowSeriesAsync(entity);

    [RelayCommand]
    public async Task LoadGamesAsync()
    {
        if (_hasReachedEnd || Series is null)
            return;

        try
        {
            PagedData<List<Game>>? games = await ExecuteNetworkTask(
                _gameSeriesService.GetGameSeriesEntriesAsync(Series.Id, _offset)
            );
            if (games is null)
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

    [RelayCommand]
    private async Task NavigateToGameAsync()
    {
        if (SelectedGame is null)
            return;

        ShowActivityIndicator();
        await Shell.Current.GoToAsync(Routes.GameDetailPageRoute, "Game", SelectedGame);
        SelectedGame = null;
    }
}
