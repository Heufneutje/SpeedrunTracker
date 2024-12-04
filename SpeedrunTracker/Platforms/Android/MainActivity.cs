using Android.App;
using Android.Content.PM;

namespace SpeedrunTracker;

[Activity(
    Theme = "@style/SpeedrunTrackerAppTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation
        | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout
        | ConfigChanges.SmallestScreenSize
        | ConfigChanges.Density
)]
public class MainActivity : MauiAppCompatActivity { }
