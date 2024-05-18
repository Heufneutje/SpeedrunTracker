namespace SpeedrunTracker;

public partial class App : Application
{
    public const string Version = "1.0.3";

    public App(ILocalSettingsService settingsService)
    {
        InitializeComponent();
        UserAppTheme = settingsService.UserSettings.Theme;
        MainPage = new AppShell();
    }
}
