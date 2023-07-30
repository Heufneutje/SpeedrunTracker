using SpeedrunTracker.Model;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

[QueryProperty(nameof(Model.User), "User")]
public partial class UserDetailPage : ContentPage
{
    private readonly UserDetailsViewModel _viewModel;
    private bool _isLoaded;

    public User User
    {
        get => _viewModel.User;
        set => _viewModel.User = value;
    }

    public UserDetailPage(UserDetailsViewModel viewModel)
    {
        InitializeComponent();
        viewModel.IsRunningBackgroundTask = true;
        BindingContext = _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
            await _viewModel.LoadPersonalBests();
    }
}
