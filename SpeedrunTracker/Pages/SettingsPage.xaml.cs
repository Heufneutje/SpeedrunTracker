using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        BindingContext = settingsViewModel;
    }
}
