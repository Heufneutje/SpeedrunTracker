using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Services;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class AboutViewModel : BaseViewModel
{
    private readonly IBrowserService _browserService;

    public ICommand OpenUrlCommand => new AsyncRelayCommand<string>(OpenUrlAsync);

    public AboutViewModel(IBrowserService browserService)
    {
        _browserService = browserService;
    }

    private async Task OpenUrlAsync(string url)
    {
        await _browserService.OpenAsync(url);
    }
}
