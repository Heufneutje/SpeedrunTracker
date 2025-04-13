using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public partial class AboutViewModel : BaseViewModel
{
    private readonly IBrowserService _browserService;

    public string VersionText => $"SpeedrunTracker {AppInfo.VersionString}";

    public AboutViewModel(IBrowserService browserService)
    {
        _browserService = browserService;
    }

    [RelayCommand]
    private async Task OpenUrlAsync(string? url)
    {
        if (!string.IsNullOrEmpty(url))
            await _browserService.OpenAsync(url);
    }
}
