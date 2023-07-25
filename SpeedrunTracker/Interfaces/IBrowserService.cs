namespace SpeedrunTracker.Interfaces;

public interface IBrowserService
{
    Task OpenAsync(string uri);
}
