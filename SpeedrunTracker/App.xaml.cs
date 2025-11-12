using SpeedrunTracker.Generated;
using SpeedrunTracker.Localization;
using System.Globalization;

namespace SpeedrunTracker;

public partial class App : Application
{
    public App(ILocalSettingsService settingsService)
    {
        InitializeComponent();
        UserAppTheme = settingsService.UserSettings.Theme;

        if (string.IsNullOrEmpty(settingsService.UserSettings.AppLanguage))
            settingsService.UserSettings.AppLanguage = SupportedLanguages.All.FirstOrDefault(x => x.Equals(CultureInfo.InstalledUICulture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase)) ?? "en-US";

        LocalizationResourceManager.Instance.Culture = new CultureInfo(settingsService.UserSettings.AppLanguage);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
