using SpeedrunTracker.Interfaces;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ILocalSettingsService _settingsService;
    private bool _hasChanges;

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

    public ThemeSetting Theme
    {
        get => Themes.FirstOrDefault(x => x.Theme == _settingsService.UserSettings.Theme);
        set
        {
            if (_settingsService.UserSettings.Theme != value.Theme)
            {
                _settingsService.UserSettings.Theme = value.Theme;
                _hasChanges = true;
                NotifyPropertyChanged();
                Application.Current.UserAppTheme = value.Theme;
            }
        }
    }

    private ObservableCollection<ThemeSetting> _themeSettings;

    public ObservableCollection<ThemeSetting> Themes
    {
        get => _themeSettings ??= new List<ThemeSetting>
        {
            new ThemeSetting("System default", AppTheme.Unspecified),
            new ThemeSetting("Light", AppTheme.Light),
            new ThemeSetting("Dark", AppTheme.Dark)
        }.AsObservableCollection();
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
