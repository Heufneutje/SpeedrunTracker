using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        BindingContext = _settingsViewModel = settingsViewModel;
    }

    private async void ContentPage_Disappearing(object sender, EventArgs e)
    {
        await _settingsViewModel.SaveChangesAsync();
    }
}
