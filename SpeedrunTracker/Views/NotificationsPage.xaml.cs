using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class NotificationsPage : ContentPage
{
    private readonly NotificationListViewModel _viewModel;
    private bool _isLoaded;

    public NotificationsPage(NotificationListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
        {
            _viewModel.IsRunningBackgroundTask = true;
            _isLoaded = true;
        }
    }
}
