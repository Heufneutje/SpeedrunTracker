using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker;

public partial class App : Application
{
    public const string Version = "0.0.1";

    public App(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        UserAppTheme = settingsViewModel.Theme.Theme;
        MainPage = new AppShell();
    }
}
