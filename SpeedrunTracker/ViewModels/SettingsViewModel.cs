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

    public ThemeSetting? Theme
    {
        get => Themes.FirstOrDefault(x => x.Theme == _settingsService.UserSettings.Theme);
        set
        {
            if (value == null || Application.Current == null)
                return;

            if (_settingsService.UserSettings.Theme != value.Theme)
            {
                _settingsService.UserSettings.Theme = value.Theme;
                _hasChanges = true;
                NotifyPropertyChanged();
                Application.Current.UserAppTheme = value.Theme;
            }
        }
    }

    public FormatSetting? DateFormat
    {
        get => DateFormats.FirstOrDefault(x => x.FormatString == _settingsService.UserSettings.DateFormat);
        set
        {
            if (value == null)
                return;

            if (_settingsService.UserSettings.DateFormat != value.FormatString)
            {
                _settingsService.UserSettings.DateFormat = value.FormatString;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    public FormatSetting? TimeFormat
    {
        get => TimeFormats.FirstOrDefault(x => x.FormatString == _settingsService.UserSettings.TimeFormat);
        set
        {
            if (value == null)
                return;

            if (_settingsService.UserSettings.TimeFormat != value.FormatString)
            {
                _settingsService.UserSettings.TimeFormat = value.FormatString;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    public bool DisplayBackgrounds
    {
        get => _settingsService.UserSettings.DisplayBackgrounds ?? false;
        set
        {
            if (_settingsService.UserSettings.DisplayBackgrounds != value)
            {
                _settingsService.UserSettings.DisplayBackgrounds = value;
                _hasChanges = true;
                NotifyPropertyChanged();
            }
        }
    }

    private ObservableCollection<ThemeSetting>? _themeSettings;

    public ObservableCollection<ThemeSetting> Themes
    {
        get => _themeSettings ??= new List<ThemeSetting>
        {
            new("System default", AppTheme.Unspecified),
            new("Light", AppTheme.Light),
            new("Dark", AppTheme.Dark)
        }.AsObservableCollection();
    }

    private ObservableCollection<FormatSetting>? _dateFormats;

    public ObservableCollection<FormatSetting> DateFormats
    {
        get => _dateFormats ??= new List<FormatSetting>()
        {
            new("1999-12-31", "yyyy-MM-dd"),
            new("31 Dec 1999", "dd MMM yyyy"),
            new("Dec 31 1999", "MMM dd yyyy")
        }.AsObservableCollection();
    }

    private ObservableCollection<FormatSetting>? _timeFormats;

    public ObservableCollection<FormatSetting> TimeFormats
    {
        get => _timeFormats ??= new List<FormatSetting>()
        {
            new("23:59", "HH:mm"),
            new("23:59:59", "HH:mm:ss"),
            new("11:59 PM", "hh:mm tt"),
            new("11:59:59 PM", "hh:mm:ss tt")
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
