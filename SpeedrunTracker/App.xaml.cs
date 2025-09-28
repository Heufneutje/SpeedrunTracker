using System.Globalization;

namespace SpeedrunTracker;

public partial class App : Application
{
    public App(ILocalSettingsService settingsService)
    {
        InitializeComponent();
        UserAppTheme = settingsService.UserSettings.Theme;

        if (!string.IsNullOrEmpty(settingsService.UserSettings.AppLanguage))
            CultureInfo.CurrentUICulture = new CultureInfo(settingsService.UserSettings.AppLanguage);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
