using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class RunDetailsViewModel : BaseNetworkActionViewModel
{
    private readonly IBrowserService _browserService;
    private readonly IUserService _userService;
    private readonly IEmbedService _embedService;

    private RunDetails _runDetails;

    public RunDetails RunDetails
    {
        get => _runDetails;
        set
        {
            _runDetails = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Title));
            NotifyPropertyChanged(nameof(HasVideo));
            NotifyPropertyChanged(nameof(HasMultipleVideos));
            NotifyPropertyChanged(nameof(HasInGameTime));
            NotifyPropertyChanged(nameof(HasRealtime));
            NotifyPropertyChanged(nameof(HasRealtimeNoLoads));
            NotifyPropertyChanged(nameof(HasTrophyAsset));
            NotifyPropertyChanged(nameof(StatusImage));
            NotifyPropertyChanged(nameof(StatusDescription));

            VideoUrls.AddRange(_embedService.GetEmbeddableUrls(value.Run.Videos));
            SelectedVideo = VideoUrls.FirstOrDefault();
        }
    }

    public bool HasVideo => _runDetails?.Run?.Videos?.Links?.Any() == true;

    public bool HasMultipleVideos => _runDetails?.Run?.Videos?.Links?.Count > 1;

    public bool HasTrophyAsset => !string.IsNullOrEmpty(_runDetails?.TrophyAsset?.FixedThemeAssetUri);

    private User _selectedPlayer;

    public User SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            if (_selectedPlayer != value)
            {
                _selectedPlayer = value;
                NotifyPropertyChanged();
            }
        }
    }

    public RangedObservableCollection<EmbeddableUrl> VideoUrls { get; set; }

    private EmbeddableUrl _selectedVideo;

    public EmbeddableUrl SelectedVideo
    {
        get => _selectedVideo;
        set
        {
            if (_selectedVideo != value)
            {
                _selectedVideo = value;
                NotifyPropertyChanged();
            }
        }
    }

    public ICommand ShowVideoCommand => new AsyncRelayCommand(ShowVideo);

    public ICommand NavigateToUserCommand => new AsyncRelayCommand<User>(NavigateToUserAsync);

    public string Title => RunDetails == null ? "RunDetails" : $"{_runDetails.Category.Name} in {_runDetails.Run.Times.PrimaryTimeSpan} by {_runDetails.Run.Players.First().DisplayName}";

    public bool HasInGameTime => ShouldShowTimingType(TimingType.InGame);

    public bool HasRealtime => ShouldShowTimingType(TimingType.Realtime);

    public bool HasRealtimeNoLoads => ShouldShowTimingType(TimingType.RealtimeNoLoads);

    public string StatusDescription
    {
        get
        {
            if (RunDetails == null)
                return null;

            return RunDetails.Run.Status.StatusType switch
            {
                SpeedrunStatusType.New => "Verification pending",
                SpeedrunStatusType.Verified => RunDetails.Run.Status.VerifyDate.HasValue ? $"Verified on {RunDetails.Run.Status.VerifyDate:yyyy-MM-dd HH:mm}" : "Verified",
                SpeedrunStatusType.Rejected => $"Rejected ({RunDetails.Run.Status.Reason ?? string.Empty})",
                _ => string.Empty,
            };
        }
    }

    public string StatusImage => (RunDetails?.Run?.Status?.StatusType) switch
    {
        SpeedrunStatusType.New => "hourglass",
        SpeedrunStatusType.Verified => "verified",
        SpeedrunStatusType.Rejected => "rejected",
        _ => string.Empty,
    };

    public RunDetailsViewModel(IBrowserService browserService, IUserService userService, IEmbedService embedService, IToastService toastService) : base(toastService)
    {
        _browserService = browserService;
        _userService = userService;
        _embedService = embedService;
        VideoUrls = new RangedObservableCollection<EmbeddableUrl>();
    }

    private async Task ShowVideo() => await _browserService.OpenAsync(SelectedVideo.Url);

    private bool ShouldShowTimingType(TimingType timingType) => RunDetails?.Ruleset?.TimingTypes?.Contains(timingType) == true && RunDetails?.Ruleset?.DefaultTimingType != timingType;

    public async Task LoadData()
    {
        if (RunDetails.Examiner == null && RunDetails.Run.Status.ExaminerId != null)
            RunDetails.Examiner = await ExecuteNetworkTask(_userService.GetUserAsync(RunDetails.Run.Status.ExaminerId)) ?? User.GetUserNotFoundPlaceholder();
    }

    private async Task NavigateToUserAsync(User user)
    {
        if (user == null)
        {
            user = SelectedPlayer;
            SelectedPlayer = null;
        }

        if (!string.IsNullOrEmpty(user.Id))
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
    }
}
