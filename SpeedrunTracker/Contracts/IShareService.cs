namespace SpeedrunTracker.Contracts;

public interface IShareService
{
    Task ShareUriAsync(ShareDetails shareDetails);
}
