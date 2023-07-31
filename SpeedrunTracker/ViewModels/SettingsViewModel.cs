using SpeedrunTracker.Interfaces;

namespace SpeedrunTracker.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ILocalSettingsService _settingsService;
    private bool _hasChanges;

    public bool EnableGameSearch
    {
        get => _settingsService.UserSettings.EnableGameSearch;
        set
        {
            if (_settingsService.UserSettings.EnableGameSearch != value)
            {
                _settingsService.UserSettings.EnableGameSearch = value;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    public bool EnableUserSearch
    {
        get => _settingsService.UserSettings.EnableUserSearch;
        set
        {
            if (_settingsService.UserSettings.EnableUserSearch != value)
            {
                _settingsService.UserSettings.EnableUserSearch = value;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    public int MaxLeaderboardResults
    {
        get => _settingsService.UserSettings.MaxLeaderboardResults;
        set
        {
            if (value < 1)
                value = 1;

            if (_settingsService.UserSettings.MaxLeaderboardResults != value)
            {
                _settingsService.UserSettings.MaxLeaderboardResults = value;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    public SettingsViewModel(ILocalSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task SaveChangesAsync()
    {
        if (_hasChanges)
            await _settingsService.SaveSettingsAsync();
    }
}
