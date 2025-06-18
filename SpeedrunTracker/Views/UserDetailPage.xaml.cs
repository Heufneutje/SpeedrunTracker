using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

[QueryProperty(nameof(User), "User")]
public partial class UserDetailPage : BaseDetailPage
{
    private readonly UserDetailsViewModel _viewModel;

    public User? User
    {
        get => _viewModel.User;
        set => _viewModel.User = value;
    }

    public UserDetailPage(UserDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
}
