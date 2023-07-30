using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SpeedrunTracker.Model.Enum;
using SpeedrunTracker.Navigation;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class UserDetailsViewModel : BaseViewModel
{
    private readonly IBrowserService _browserService;
    private readonly IUserRepository _userRepository;
    private User _user;

    public User User
    {
        get => _user;
        set
        {
            if (_user != value)
            {
                _user = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DisplayName));
                NotifyPropertyChanged(nameof(CountryImageSource));
                NotifyPropertyChanged(nameof(HasPronouns));
                NotifyPropertyChanged(nameof(HasYouTube));
                NotifyPropertyChanged(nameof(HasTwitch));
                NotifyPropertyChanged(nameof(HasTwitter));
                NotifyPropertyChanged(nameof(HasHitbox));
                NotifyPropertyChanged(nameof(HasSpeedRunsLive));
            }
        }
    }

    private ObservableCollection<UserPersonalBestsGroup> _personalBests;

    public ObservableCollection<UserPersonalBestsGroup> PersonalBests
    {
        get => _personalBests;
        set
        {
            if (_personalBests != value)
            {
                _personalBests = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string DisplayName => _user?.DisplayName;

    public string CountryImageSource => $"flags/{_user?.Location?.Country?.Code}_flag";

    public bool HasPronouns => !string.IsNullOrEmpty(_user?.Pronouns);

    public bool HasYouTube => !string.IsNullOrEmpty(_user?.YouTube?.Uri);

    public bool HasTwitch => !string.IsNullOrEmpty(_user?.Twitch?.Uri);

    public bool HasTwitter => !string.IsNullOrEmpty(_user?.Twitter?.Uri);

    public bool HasHitbox => !string.IsNullOrEmpty(_user?.Hitbox?.Uri);

    public bool HasSpeedRunsLive => !string.IsNullOrEmpty(_user?.SpeedRunsLive?.Uri);

    public ICommand OpenUrlCommand => new AsyncRelayCommand<string>(OpenUrl);

    public ICommand NavigateToRunCommand => new AsyncRelayCommand<LeaderboardEntry>(NavigateToRun);

    public UserDetailsViewModel(IBrowserService browserService, IUserRepository userRepository)
    {
        _browserService = browserService;
        _userRepository = userRepository;
    }

    public async Task LoadPersonalBests()
    {
        List<UserPersonalBestsGroup> groups = new();
        IEnumerable<IGrouping<string, LeaderboardEntry>> groupedBests = (await _userRepository.GetUserPersonalBestsAsync(_user.Id)).Data.GroupBy(x => x.Run.GameId);
        Dictionary<string, User> players = new();
        foreach (IGrouping<string, LeaderboardEntry> group in groupedBests)
        {
            List<LeaderboardEntry> entries = group.ToList();
            foreach (LeaderboardEntry entry in entries)
            {
                foreach (KeyValuePair<string, string> valuePair in entry.Run.Values)
                {
                    Variable variable = entry.Category.Data.Variables.Data.FirstOrDefault(x => x.Id == valuePair.Key);
                    if (variable == null || !variable.Values.Values.ContainsKey(valuePair.Value))
                        continue;

                    VariableValue value = variable.Values.Values[valuePair.Value];
                    entry.Run.Variables.Add(new(variable.Name, value.Name, variable.IsSubcategory));
                }

                for (int i = 0; i < entry.Run.Players.Count; i++)
                {
                    if (entry.Run.Players[i].PlayerType != PlayerType.User)
                        continue;

                    string playerId = entry.Run.Players[i].Id;
                    if (playerId == _user.Id)
                        entry.Run.Players[i] = _user;
                    else
                    {
                        if (!players.ContainsKey(playerId))
                            players.Add(playerId, (await _userRepository.GetUserAsync(playerId)).Data);

                        entry.Run.Players[i] = players[playerId];
                    }
                }
            }

            groups.Add(new UserPersonalBestsGroup(entries.First().Game.Data, entries));
        }

        PersonalBests = groups.AsObservableCollection();
        IsRunningBackgroundTask = false;
    }

    private async Task NavigateToRun(LeaderboardEntry entry)
    {
        User examiner = null;
        if (entry.Run.Status.ExaminerId != null)
            examiner = (await _userRepository.GetUserAsync(entry.Run.Status.ExaminerId)).Data;

        RunDetails runDetails = new RunDetails()
        {
            Category = entry.Category.Data,
            GameAssets = entry.Game.Data.Assets,
            Examiner = examiner,
            Level = entry.Level,
            Place = entry.Place,
            Platform = entry.Platform.Data,
            Run = entry.Run,
            Variables = entry.Run.Variables
        };

        await Shell.Current.GoToAsync(Routes.RunDetailPageRoute, "RunDetails", runDetails);
    }

    private async Task OpenUrl(string url)
    {
        await _browserService.OpenAsync(url);
    }
}
