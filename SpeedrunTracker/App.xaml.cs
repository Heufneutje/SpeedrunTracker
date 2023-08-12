using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker;

public partial class App : Application
{
    public const string Version = "1.0.0";

    public App(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        UserAppTheme = settingsViewModel.Theme.Theme;
        MainPage = new AppShell();
    }
}
