using SpeedrunTracker.Model;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

[QueryProperty(nameof(Game), "Game")]
public partial class GameDetailPage : ContentPage
{
    private readonly GameDetailViewModel _viewModel;
    private bool _isLoaded;

    public Game Game
    {
        get => _viewModel.Game;
        set => _viewModel.Game = value;
    }

    public GameDetailPage(GameDetailViewModel viewModel)
    {
        InitializeComponent();
        viewModel.IsRunningBackgroundTask = true;
        BindingContext = _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
        {
            await _viewModel.LoadVariablesAsync();
            await _viewModel.LoadCategoriesAsync();
            await _viewModel.LoadLevelsAsync();
            _viewModel.IsRunningBackgroundTask = false;
            _isLoaded = true;
        }
    }
}
