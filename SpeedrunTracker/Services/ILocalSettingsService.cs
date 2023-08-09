namespace SpeedrunTracker.Services;

public interface ILocalSettingsService
{
    UserSettings UserSettings { get; }

    Task LoadSettingsAsync();

    Task SaveSettingsAsync();
}
