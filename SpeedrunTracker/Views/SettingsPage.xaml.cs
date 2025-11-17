using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        BindingContext = settingsViewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        themeCell.Reload();
    }
}
