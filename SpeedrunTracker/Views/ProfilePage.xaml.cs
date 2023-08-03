using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;
    private bool _isLoaded;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
        {
            await _viewModel.LoadProfileAsync();
            _isLoaded = true;
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        _apiKeyEntry.IsEnabled = false;
        _apiKeyEntry.IsEnabled = true;
    }
}
