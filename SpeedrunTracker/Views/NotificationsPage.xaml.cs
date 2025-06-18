using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class NotificationsPage : ContentPage
{
    public NotificationsPage(NotificationListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
