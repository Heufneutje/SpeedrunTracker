using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker;

public partial class App : Application
{
    public App(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        UserAppTheme = settingsViewModel.Theme.Theme;
        MainPage = new AppShell();
    }
}
