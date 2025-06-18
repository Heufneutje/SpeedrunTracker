using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

[QueryProperty(nameof(Game), "Game")]
public partial class GameDetailPage : BaseDetailPage
{
    private readonly GameDetailViewModel _viewModel;

    public Game? Game
    {
        get => _viewModel.Game;
        set => _viewModel.Game = value;
    }

    public GameDetailPage(GameDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
}
