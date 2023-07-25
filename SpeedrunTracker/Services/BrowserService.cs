using SpeedrunTracker.Interfaces;

namespace SpeedrunTracker.Services;

public class BrowserService : IBrowserService
{
    public Task OpenAsync(string uri)
    {
        return Browser.Default.OpenAsync(uri);
    }
}
