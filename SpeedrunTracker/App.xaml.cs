﻿namespace SpeedrunTracker;

public partial class App : Application
{
    public const string Version = "1.2.0";

    public App(ILocalSettingsService settingsService)
    {
        InitializeComponent();
        UserAppTheme = settingsService.UserSettings.Theme;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
