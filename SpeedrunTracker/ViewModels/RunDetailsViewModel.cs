﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class RunDetailsViewModel : BaseShareableViewModel
{
    private readonly IBrowserService _browserService;
    private readonly IUserService _userService;
    private readonly IEmbedService _embedService;
    private readonly IConfiguration _config;
    private readonly ILocalSettingsService _settingsService;
    private RunDetails? _runDetails;

    public RunDetails? RunDetails
    {
        get => _runDetails;
        set
        {
            _runDetails = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(BackgroundUri));

            if (value?.Run.Videos?.Links != null)
                VideoUrls.AddRange(_embedService.GetEmbeddableUrls(value.Run.Videos));

            SelectedVideo = VideoUrls.FirstOrDefault();
        }
    }

    public bool HasVideo => _runDetails?.Run?.Videos?.Links?.Any() == true;

    public bool HasMultipleVideos => _runDetails?.Run?.Videos?.Links?.Count > 1;

    public bool HasTrophyAsset => !string.IsNullOrEmpty(_runDetails?.TrophyAsset?.Uri);

    private string? _runComment;

    public string? RunComment
    {
        get
        {
            if (_runComment == null && RunDetails?.Run?.Comment != null)
                _runComment = RunDetails
                    .Run.Comment.Replace("(/static/blob", $"({_config["speedrun-dot-com:base-address"]}static/blob")
                    .MarkdownifyUrls();
            return _runComment;
        }
    }

    private User? _selectedPlayer;

    public User? SelectedPlayer
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

    private EmbeddableUrl? _selectedVideo;

    public EmbeddableUrl? SelectedVideo
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

    public ICommand OpenLinkCommand => new AsyncRelayCommand<string>(OpenLinkAsync);

    public string? Title =>
        _runDetails == null
            ? "RunDetails"
            : $"{_runDetails.Category?.Name} in {_runDetails.Run.Times?.PrimaryTimeSpan}";

    public string? SubTitle =>
        _runDetails == null
            ? "by <unknown>"
            : $"by {string.Join(" and ", _runDetails.Run.Players.Select(x => x.DisplayName))}";

    public string FullTitle => $"{Title} {SubTitle}";

    public string? FormattedDate => RunDetails?.Run.Date?.ToString(_settingsService.UserSettings.DateFormat);

    public bool HasInGameTime => ShouldShowTimingType(TimingType.InGame);

    public bool HasRealtime => ShouldShowTimingType(TimingType.Realtime);

    public bool HasRealtimeNoLoads => ShouldShowTimingType(TimingType.RealtimeNoLoads);

    public string? StatusDescription
    {
        get
        {
            if (RunDetails == null)
                return null;

            return RunDetails.Run.Status?.StatusType switch
            {
                SpeedrunStatusType.New => "Verification pending",
                SpeedrunStatusType.Verified => RunDetails.Run.Status.VerifyDate.HasValue
                    ? $"Verified on {RunDetails.Run.Status.VerifyDate.Value.ToString(_settingsService.UserSettings.DateFormat)} at {RunDetails.Run.Status.VerifyDate.Value.ToString(_settingsService.UserSettings.TimeFormat)}"
                    : "Verified",
                SpeedrunStatusType.Rejected => $"Rejected ({RunDetails.Run.Status.Reason ?? string.Empty})",
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
        _settingsService.UserSettings.DisplayBackgrounds == true ? RunDetails?.GameAssets?.Background?.Uri : null;

    public override ShareDetails ShareDetails => new(RunDetails?.Run?.Weblink, Title);

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

    private async Task ShowVideo()
    {
        if (!string.IsNullOrEmpty(_selectedVideo?.Url))
            await _browserService.OpenAsync(_selectedVideo.Url);
    }

    public async Task LoadData()
    {
        if (RunDetails?.Examiner == null && RunDetails?.Run.Status?.ExaminerId != null)
            RunDetails.Examiner =
                await ExecuteNetworkTask(_userService.GetUserAsync(RunDetails.Run.Status.ExaminerId))
                ?? User.GetUserNotFoundPlaceholder();

        if (!HasVideo)
            CloseActivityIndicator();
    }

    private async Task NavigateToUserAsync(User? user)
    {
        if (user == null)
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

    private async Task OpenLinkAsync(string? uri)
    {
        if (!string.IsNullOrEmpty(uri))
            await _browserService.OpenAsync(uri);
    }
}
