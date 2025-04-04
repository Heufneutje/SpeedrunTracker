using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public class AboutViewModel : BaseViewModel
{
    private readonly IBrowserService _browserService;

    public ICommand OpenUrlCommand => new AsyncRelayCommand<string>(OpenUrlAsync);

    public string VersionText => $"SpeedrunTracker {AppInfo.VersionString}";

    public AboutViewModel(IBrowserService browserService)
    {
        _browserService = browserService;
    }

    private async Task OpenUrlAsync(string? url)
    {
        if (!string.IsNullOrEmpty(url))
            await _browserService.OpenAsync(url);
    }
}
