namespace SpeedrunTracker.Services;

public interface IShareService
{
    Task ShareUriAsync(ShareDetails shareDetails);
}
