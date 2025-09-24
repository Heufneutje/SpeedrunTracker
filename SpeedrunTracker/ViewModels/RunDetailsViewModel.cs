using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class RunDetailsViewModel : BaseShareableViewModel
{
    private readonly IBrowserService _browserService;
    private readonly IUserService _userService;
    private readonly IEmbedService _embedService;
    private readonly IConfiguration _config;
    private readonly ILocalSettingsService _settingsService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundUri))]
    private RunDetails? _runDetails;

    public bool HasVideo => RunDetails?.Run?.Videos?.Links?.Count > 0;

    public bool HasMultipleVideos => RunDetails?.Run?.Videos?.Links?.Count > 1;

    public bool HasTrophyAsset => !string.IsNullOrEmpty(RunDetails?.TrophyAsset?.SecureUri);

    private string? _runComment;

    public string? RunComment
    {
        get
        {
            if (_runComment is null && RunDetails?.Run?.Comment is not null)
                _runComment = RunDetails
                    .Run.Comment.Replace("(/static/blob", $"({_config["speedrun-dot-com:base-address"]}static/blob")
                    .MarkdownifyUrls();
            return _runComment;
        }
    }

    [ObservableProperty]
    private User? _selectedPlayer;

    public RangedObservableCollection<EmbeddableUrl> VideoUrls { get; set; }

    [ObservableProperty]
    private EmbeddableUrl? _selectedVideo;

    public string? Title =>
        RunDetails is null
            ? "RunDetails"
            : $"{RunDetails.Category?.Name} {AppStrings.RunDetailsPageTitleInText} {RunDetails.Run.Times?.PrimaryTimeSpan}";

    public string? SubTitle =>
        RunDetails is null
            ? AppStrings.RunDetailsPageSubtitleByUnknownText
            : $"{AppStrings.RunDetailsPageSubtitleByText} {string.Join($" {AppStrings.RunDetailsPageSubtitlePlayerSeparator} ", RunDetails.Run.Players.Select(x => x.DisplayName))}";

    public string FullTitle => $"{Title} {SubTitle}";

    public string? FormattedDate => RunDetails?.Run.Date?.ToString(_settingsService.UserSettings.DateFormat);

    public bool HasInGameTime => ShouldShowTimingType(TimingType.InGame);

    public bool HasRealtime => ShouldShowTimingType(TimingType.Realtime);

    public bool HasRealtimeNoLoads => ShouldShowTimingType(TimingType.RealtimeNoLoads);

    public string? StatusDescription
    {
        get
        {
            if (RunDetails is null)
                return null;

            return RunDetails.Run.Status?.StatusType switch
            {
                SpeedrunStatusType.New => AppStrings.RunDetailsPageStatusVerificationPendingText,
                SpeedrunStatusType.Verified => RunDetails.Run.Status.VerifyDate.HasValue
                    ? string.Format(AppStrings.RunDetailsPageStatusVerifiedOnText, RunDetails.Run.Status.VerifyDate.Value.ToString(_settingsService.UserSettings.DateFormat), RunDetails.Run.Status.VerifyDate.Value.ToString(_settingsService.UserSettings.TimeFormat))
                    : AppStrings.RunDetailsPageStatusVerifiedText,
                SpeedrunStatusType.Rejected => $"{AppStrings.RunDetailsPageStatusRejectedText} ({RunDetails.Run.Status.Reason ?? string.Empty})",
                _ => string.Empty,
            };
        }
    }

    public string StatusImage =>
        (RunDetails?.Run?.Status?.StatusType) switch
        {
            SpeedrunStatusType.New => "hourglass",
            SpeedrunStatusType.Verified => "verified",
            SpeedrunStatusType.Rejected => "rejected",
            _ => string.Empty,
        };

    private bool ShouldShowTimingType(TimingType timingType) =>
        RunDetails?.Ruleset?.TimingTypes?.Contains(timingType) == true
        && RunDetails?.Ruleset?.DefaultTimingType != timingType;

    public string? BackgroundUri =>
        _settingsService.UserSettings.DisplayBackgrounds == true ? RunDetails?.GameAssets?.Background?.SecureUri : null;

    public override ShareDetails ShareDetails => new(RunDetails?.Run?.SecureWeblink, Title);

    public RunDetailsViewModel(
        IBrowserService browserService,
        IUserService userService,
        IEmbedService embedService,
        IShareService shareService,
        IToastService toastService,
        IConfiguration config,
        ILocalSettingsService settingsService,
        IPopupService popupService
    )
        : base(shareService, toastService, popupService)
    {
        _browserService = browserService;
        _userService = userService;
        _embedService = embedService;
        _config = config;
        _settingsService = settingsService;
        VideoUrls = [];
    }

    partial void OnRunDetailsChanged(RunDetails? value)
    {
        if (value?.Run.Videos?.Links is not null)
            VideoUrls.AddRange(_embedService.GetEmbeddableUrls(value.Run.Videos));

        SelectedVideo = VideoUrls.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ShowVideoAsync()
    {
        if (!string.IsNullOrEmpty(SelectedVideo?.Url))
            await _browserService.OpenAsync(SelectedVideo.Url);
    }

    public async Task LoadDataAsync()
    {
        if (RunDetails?.Examiner is null && RunDetails?.Run.Status?.ExaminerId is not null)
            RunDetails.Examiner =
                await ExecuteNetworkTask(_userService.GetUserAsync(RunDetails.Run.Status.ExaminerId))
                ?? User.GetUserNotFoundPlaceholder();

        if (!HasVideo)
            CloseActivityIndicator();
    }

    [RelayCommand]
    private async Task NavigateToUserAsync(User? user)
    {
        if (user is null)
        {
            user = SelectedPlayer;
            SelectedPlayer = null;
        }

        if (!string.IsNullOrEmpty(user?.Id))
        {
            ShowActivityIndicator();
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
        }
    }

    [RelayCommand]
    private async Task OpenLinkAsync(string? uri)
    {
        if (!string.IsNullOrEmpty(uri))
            await _browserService.OpenAsync(uri);
    }
}
