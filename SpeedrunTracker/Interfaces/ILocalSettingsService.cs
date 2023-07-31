namespace SpeedrunTracker.Interfaces;

public interface ILocalSettingsService
{
    UserSettings UserSettings { get; }

    Task LoadSettingsAsync();

    Task SaveSettingsAsync();
}
