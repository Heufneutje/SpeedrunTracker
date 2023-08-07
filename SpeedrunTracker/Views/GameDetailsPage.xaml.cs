using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

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
        if (_isLoaded)
            return;

        try
        {
            if (!await _viewModel.LoadVariablesAsync())
            {
                await NagivateBack();
                return;
            }

            if (!await _viewModel.LoadCategoriesAsync())
            {
                await NagivateBack();
                return;
            }

            if (!await _viewModel.LoadLevelsAsync())
            {
                await NagivateBack();
                return;
            }

            await _viewModel.LoadFollowingStatusAsync();
            _isLoaded = true;
        }
        finally
        {
            _viewModel.IsRunningBackgroundTask = false;
        }
    }

    private async Task NagivateBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
