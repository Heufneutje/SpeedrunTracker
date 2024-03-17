namespace SpeedrunTracker.Contracts.LocalStorage;

public interface ILocalSettingsService
{
    UserSettings UserSettings { get; }

    Task LoadSettingsAsync();

    Task SaveSettingsAsync();
}
