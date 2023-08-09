namespace SpeedrunTracker.Services;

public interface IBrowserService
{
    Task OpenAsync(string uri);
}
