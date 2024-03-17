namespace SpeedrunTracker.Contracts;

public interface IEmbedService
{
    List<EmbeddableUrl> GetEmbeddableUrls(SpeedrunVideos videos);
}
