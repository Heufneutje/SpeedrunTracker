namespace SpeedrunTracker.Contracts;

public interface IBrowserService
{
    Task OpenAsync(string uri);
}
