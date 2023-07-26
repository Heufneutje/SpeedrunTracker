namespace SpeedrunTracker.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private bool _enableGameSearch;

    public bool EnableGameSearch
    {
        get => _enableGameSearch;
        set
        {
            _enableGameSearch = value;
            NotifyPropertyChanged();
        }
    }

    private bool _enableUserSearch;

    public bool EnableUserSearch
    {
        get => _enableUserSearch;
        set
        {
            _enableUserSearch = value;
            NotifyPropertyChanged();
        }
    }

    private int _maxLeaderboardResults;

    public int MaxLeaderboardResults
    {
        get => _maxLeaderboardResults;
        set
        {
            if (value < 1)
                value = 1;

            _maxLeaderboardResults = value;
            NotifyPropertyChanged();
        }
    }

    public SettingsViewModel()
    {
        EnableGameSearch = true;
        EnableUserSearch = true;
        MaxLeaderboardResults = 50;
    }
}
